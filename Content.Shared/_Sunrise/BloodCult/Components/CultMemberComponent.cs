using Robust.Shared.GameStates;
using Robust.Shared.Localization;

namespace Content.Shared._Sunrise.BloodCult.Components;

/// <summary>
/// This is used for tagging a mob as a cultist.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class CultMemberComponent : Component
{
    [DataField]
    public EntityUid? LastAttackedEntity = null;

    [DataField]
    public TimeSpan? NextPopupTime = null;

    [DataField]
    public TimeSpan PopupCooldown = TimeSpan.FromSeconds(3.0);

    [DataField]
    public LocId Reason = "cult-member-cannot-attack"; // Fire edit - Локализация причины запрета атаки.
}
