using Content.Shared.Maps;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Impstation.Mummy.TileReplaceRadiusOnTrigger;

[RegisterComponent, NetworkedComponent]
public sealed partial class TileReplaceRadiusOnTriggerComponent : Component
{
    [DataField]
    public float Radius = 2.5f;

    [DataField]
    public float ConvertChancePerTile = 0.8f;

    [DataField]
    public ProtoId<ContentTileDefinition> ConversionTile = "FloorAsteroidSand"; // TEMP
    [DataField]
    public EntProtoId? TileConversionVfx = null; // TEMP
}
