using Robust.Shared.Localization;

namespace Content.Shared.Maps;

public sealed partial class GameMapPrototype
{
    /// <summary>
    /// Optional localized replacement for <see cref="MapName"/>.
    /// </summary>
    [DataField]
    public LocId? MapNameLocId { get; private set; }

    /// <summary>
    /// Returns the localized map name, or the literal map name when no translation is available.
    /// </summary>
    public string GetLocalizedName(ILocalizationManager localization)
    {
        return MapNameLocId is { } locId && localization.TryGetString(locId, out var localized)
            ? localized
            : MapName;
    }
}
