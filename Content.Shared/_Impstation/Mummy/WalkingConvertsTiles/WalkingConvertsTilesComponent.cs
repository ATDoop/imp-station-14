using Content.Shared.Maps;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Shared._Impstation.Mummy.WalkingConvertsTiles;

[RegisterComponent, NetworkedComponent]
public sealed partial class WalkingConvertsTilesComponent : Component
{
    [DataField]
    public float ConvertChancePerTile = 1f;

    [DataField]
    public ProtoId<ContentTileDefinition> ConversionTile = "FloorAsteroidSand"; // TEMP
    [DataField]
    public EntProtoId? TileConversionVfx = null; // TEMP

    public TileRef? LastTile = null;
}
