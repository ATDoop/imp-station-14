using Content.Server.GameTicking;
using Content.Shared.Inventory.Events;

namespace Content.Server._Impstation.Mummy.AddGameruleOnEquip;

public sealed class AddGameruleOnEquipSystem : EntitySystem
{
    [Dependency] private readonly GameTicker _gameTicker = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AddGameruleOnEquipComponent, GotEquippedEvent>(OnEquipped);
    }

    private void OnEquipped(Entity<AddGameruleOnEquipComponent> ent, ref GotEquippedEvent args)
    {
        if (ent.Comp.OneShot && ent.Comp.HasRun)
            return;

        var rule = _gameTicker.AddGameRule(ent.Comp.GameRule);
        _gameTicker.StartGameRule(rule);
        ent.Comp.HasRun = true;
    }
}
