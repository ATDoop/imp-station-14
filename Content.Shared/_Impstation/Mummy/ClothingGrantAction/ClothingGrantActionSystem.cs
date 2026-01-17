using Content.Shared.Actions;

namespace Content.Shared._Impstation.Mummy.ClothingGrantAction;

public sealed class ClothingGrantActionSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ClothingGrantActionComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<ClothingGrantActionComponent, GetItemActionsEvent>(OnGetActions);
    }

    private void OnMapInit(Entity<ClothingGrantActionComponent> ent, ref MapInitEvent args)
    {
        if (string.IsNullOrEmpty(ent.Comp.Action))
            return;

        _actions.AddAction(ent, ref ent.Comp.ActionEntity, ent.Comp.Action);
        Dirty(ent);
    }

    private void OnGetActions(Entity<ClothingGrantActionComponent> ent, ref GetItemActionsEvent args)
    {
        if (args.InHands && ent.Comp.MustEquip)
            return;

        args.AddAction(ent.Comp.ActionEntity);
    }
}
