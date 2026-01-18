using Content.Shared.GameTicking.Components;
using Robust.Shared.Prototypes;

namespace Content.Server._Impstation.Mummy.AddGameruleOnEquip;

[RegisterComponent]
public sealed partial class AddGameruleOnEquipComponent : Component
{
    [DataField(required: true)]
    public EntProtoId<GameRuleComponent> GameRule;

    [DataField]
    public bool OneShot = true;

    public bool HasRun = false;
}
