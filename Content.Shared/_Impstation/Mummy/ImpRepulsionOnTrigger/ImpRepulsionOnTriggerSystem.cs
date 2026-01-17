using Content.Shared.Physics;
using Content.Shared.Throwing;
using Robust.Shared.Random;
using Robust.Shared.Physics.Components;
using Content.Shared.Trigger;

namespace Content.Shared._Impstation.Mummy.ImpRepulsion;

public sealed class ImpRepulsionOnTriggerSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly SharedTransformSystem _xform = default!;
    [Dependency] private readonly ThrowingSystem _throwing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ImpRepulsionOnTriggerComponent, TriggerEvent>(OnTrigger);
    }

    private void OnTrigger(Entity<ImpRepulsionOnTriggerComponent> ent, ref TriggerEvent args)
    {
        var xform = Transform(ent);
        var xformQuery = GetEntityQuery<TransformComponent>();
        var worldPos = _xform.GetWorldPosition(xform, xformQuery);

        var lookup = _lookup.GetEntitiesInRange(ent, ent.Comp.ThrowRadius, LookupFlags.Dynamic | LookupFlags.Sundries);
        var physQuery = GetEntityQuery<PhysicsComponent>();

        foreach (var obj in lookup)
        {
            if (physQuery.TryGetComponent(ent, out var phys)
                && (phys.CollisionMask & (int)CollisionGroup.GhostImpassable) != 0)
                continue;

            var objWorldPos = _xform.GetWorldPosition(obj, xformQuery);
            var throwDirection = objWorldPos - worldPos;
            if (throwDirection.Length() < ent.Comp.ThrowDeadzone)
                continue;
            _throwing.TryThrow(obj, throwDirection.Normalized() * (ent.Comp.MaxThrowStrength / 2), _random.NextFloat(ent.Comp.MinThrowStrength, ent.Comp.MinThrowStrength), ent, 0);
        }
    }
}
