using System.Numerics;
using Content.Shared.Maps;
using Content.Shared.Trigger;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Random;

namespace Content.Shared._Impstation.Mummy.TileReplaceRadiusOnTrigger;

public sealed class TileReplaceRadiusOnTriggerSystem : EntitySystem
{
    [Dependency] private readonly ITileDefinitionManager _tileDef = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly TileSystem _tile = default!;
    [Dependency] private readonly TurfSystem _turf = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TileReplaceRadiusOnTriggerComponent, TriggerEvent>(OnTrigger);
    }

    private void OnTrigger(Entity<TileReplaceRadiusOnTriggerComponent> ent, ref TriggerEvent args)
    {
        var xform = Transform(ent);
        if (xform.GridUid is not { } gridUid || !TryComp(gridUid, out MapGridComponent? mapGrid))
            return;

        var pos = xform.Coordinates.Position;
        var radius = ent.Comp.Radius;
        var radiusBox = new Box2(
            pos + new Vector2(-radius, -radius),
            pos + new Vector2(radius, radius)
        );

        var tileEnumerator = _map.GetLocalTilesEnumerator(gridUid, mapGrid, radiusBox);
        var convertTile = (ContentTileDefinition)_tileDef[ent.Comp.ConversionTile];

        while (tileEnumerator.MoveNext(out var tile))
        {
            if (tile.Tile.TypeId == convertTile.TileId)
                continue;

            var tileCoords = tile.GridIndices;

            // have to check the distance from center to tileref, otherwise it comes out square due to Box2
            if (Math.Sqrt(Math.Pow(tileCoords.X - (pos.X - 0.5), 2) + Math.Pow(tileCoords.Y - (pos.Y - 0.5), 2)) >= radius)
                continue;

            if (_random.Prob(ent.Comp.ConvertChancePerTile))
            {
                var center = _turf.GetTileCenter(tile);
                if (ent.Comp.TileConversionVfx != null)
                    Spawn(ent.Comp.TileConversionVfx, center);

                _tile.ReplaceTile(tile, convertTile);
                _tile.PickVariant(convertTile);
            }
        }
    }
}
