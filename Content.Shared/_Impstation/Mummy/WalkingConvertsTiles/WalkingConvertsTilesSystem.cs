using Content.Shared.Maps;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Network;
using Robust.Shared.Random;

namespace Content.Shared._Impstation.Mummy.WalkingConvertsTiles;

public sealed class WalkingConvertsTilesSystem : EntitySystem
{
    [Dependency] private readonly INetManager _netMan = default!;
    [Dependency] private readonly ITileDefinitionManager _tileDef = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    [Dependency] private readonly SharedMapSystem _mapSystem = default!;
    [Dependency] private readonly TileSystem _tile = default!;
    [Dependency] private readonly TurfSystem _turf = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var enumerator = EntityQueryEnumerator<WalkingConvertsTilesComponent>();

        while (enumerator.MoveNext(out var uid, out var comp))
        {
            if (_netMan.IsClient)
                continue;

            var xform = Transform(uid);
            if (xform.GridUid == null)
                continue;

            if (!TryComp<MapGridComponent>(xform.GridUid, out var grid))
                continue;

            var tile = _mapSystem.GetTileRef(xform.GridUid.Value, grid, xform.Coordinates);

            if (tile == TileRef.Zero || tile == comp.LastTile)
                continue;

            if (_random.Prob(comp.ConvertChancePerTile))
            {
                var center = _turf.GetTileCenter(tile);
                if (comp.TileConversionVfx != null)
                    Spawn(comp.TileConversionVfx, center);

                var convertTile = (ContentTileDefinition)_tileDef[comp.ConversionTile];
                _tile.ReplaceTile(tile, convertTile);
                _tile.PickVariant(convertTile);
                tile = _mapSystem.GetTileRef(xform.GridUid.Value, grid, xform.Coordinates);
            }

            comp.LastTile = tile;
        }
    }
}
