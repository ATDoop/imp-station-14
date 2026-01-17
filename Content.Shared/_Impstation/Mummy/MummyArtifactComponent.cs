using Robust.Shared.GameStates;

namespace Content.Shared._Impstation.Mummy.MummyArtifactComponent;

/// <summary>
/// Empty tag component for the Mummy's Crown, used to track it on the pinpointer.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class MummyArtifactComponent : Component;
