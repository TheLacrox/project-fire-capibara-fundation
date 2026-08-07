using Robust.Shared.Localization;

namespace Content.Server.Station.Components;

public sealed partial class StationNameSetupComponent
{
    /// <summary>
    /// Optional localized replacement for <see cref="StationNameTemplate"/>.
    /// </summary>
    [DataField]
    public LocId? StationNameTemplateLocId { get; private set; }
}
