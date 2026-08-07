using Robust.Shared.Localization;

namespace Content.Server.Station.Systems;

public sealed partial class StationNameSystem
{
    internal static string ResolveStationNameTemplate(
        string literalTemplate,
        LocId? templateLocId,
        ILocalizationManager localization)
    {
        return templateLocId is { } locId && localization.TryGetString(locId, out var localizedTemplate)
            ? localizedTemplate
            : literalTemplate;
    }
}
