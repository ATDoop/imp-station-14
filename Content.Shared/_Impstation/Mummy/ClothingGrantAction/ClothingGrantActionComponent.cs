using Content.Shared.Actions.Components;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Impstation.Mummy.ClothingGrantAction;

/// <summary>
/// Clothing that grants an arbitrary action when equipped.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ClothingGrantActionComponent : Component
{
    [DataField(required: true)]
    public EntProtoId<InstantActionComponent> Action;

    [DataField, AutoNetworkedField]
    public EntityUid? ActionEntity;

    [DataField]
    public bool MustEquip = true;
}
