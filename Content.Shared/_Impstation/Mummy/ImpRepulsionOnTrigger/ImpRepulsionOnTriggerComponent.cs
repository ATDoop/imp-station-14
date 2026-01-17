using Robust.Shared.GameStates;

namespace Content.Shared._Impstation.Mummy.ImpRepulsion;

[RegisterComponent, NetworkedComponent]
public sealed partial class ImpRepulsionOnTriggerComponent : Component
{
    [DataField]
    public float ThrowRadius = 3f;

    [DataField]
    public float ThrowDeadzone = 0.8f;

    [DataField]
    public float MinThrowStrength = 10f;
    [DataField]
    public float MaxThrowStrength = 15f;
}
