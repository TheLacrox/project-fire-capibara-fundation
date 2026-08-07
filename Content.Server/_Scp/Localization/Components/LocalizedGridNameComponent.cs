using Robust.Shared.GameObjects;
using Robust.Shared.Localization;

namespace Content.Server._Scp.Localization.Components;

/// <summary>
/// Stores the localization key used for the owning grid's visible name.
/// </summary>
[RegisterComponent]
public sealed partial class LocalizedGridNameComponent : Component
{
    /// <summary>
    /// Localization key applied when the component starts.
    /// </summary>
    [DataField(required: true)]
    public LocId Name;
}
