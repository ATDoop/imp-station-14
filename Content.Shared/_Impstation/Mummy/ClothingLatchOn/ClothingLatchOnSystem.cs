using Content.Shared.Hands;
using Content.Shared.Interaction.Components;
using Content.Shared.Inventory;
using Content.Shared.Mobs;
using Content.Shared.Movement.Pulling.Events;
using Content.Shared.Popups;

namespace Content.Shared._Impstation.Mummy.ClothingLatchOn;

public sealed class SharedClothingLatchOnSystem : EntitySystem
{
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ClothingLatchOnComponent, GotEquippedHandEvent>(OnEquippedHand);
        SubscribeLocalEvent<ClothingLatchOnComponent, PullAttemptEvent>(OnPullAttempt);
        SubscribeLocalEvent<ClothingLatchOnComponent, InventoryRelayedEvent<MobStateChangedEvent>>(OnEquippedMobStateChanged);
    }

    public void TryLatchOn(Entity<ClothingLatchOnComponent> ent, EntityUid target)
    {
        if (_inventory.TryGetSlotEntity(target, ent.Comp.TargetSlot, out var slotEntity) && slotEntity != null)
            _inventory.TryUnequip(target, ent.Comp.TargetSlot, false, true);

        if (!_inventory.TryEquip(target, ent, ent.Comp.TargetSlot, false, true))
            return;

        EnsureComp<UnremoveableComponent>(ent);
        ent.Comp.LatchedEnt = target;

        _popup.PopupClient(Loc.GetString(ent.Comp.LatchOnPopup), target, PopupType.LargeCaution);
    }

    public void OnEquippedHand(Entity<ClothingLatchOnComponent> ent, ref GotEquippedHandEvent args)
    {
        if (ent.Comp.LatchedEnt != null)
            return;
        TryLatchOn(ent, args.User);
    }

    public void OnPullAttempt(Entity<ClothingLatchOnComponent> ent, ref PullAttemptEvent args)
    {
        if (ent.Comp.LatchedEnt != null)
            return;
        args.Cancelled = true;
        TryLatchOn(ent, args.PullerUid);
    }

    public void OnEquippedMobStateChanged(Entity<ClothingLatchOnComponent> ent, ref InventoryRelayedEvent<MobStateChangedEvent> args)
    {
        if (args.Args.NewMobState == MobState.Alive)
            return;

        if (HasComp<UnremoveableComponent>(ent))
            RemComp<UnremoveableComponent>(ent);

        ent.Comp.LatchedEnt = null;
    }
}
