using Content.Server.Station.Components;
using Content.Shared.Station.Components;
using Robust.Shared.Localization;

namespace Content.Server.Station.Systems;

/// <summary>
/// This handles naming stations.
/// </summary>
public sealed partial class StationNameSystem : EntitySystem // Fire edit - локализуемые шаблоны названий станции
{
    [Dependency] private readonly StationSystem _station = default!;
    [Dependency] private readonly ILocalizationManager _localization = default!; // Fire added - локализуемые шаблоны названий станции

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<StationNameSetupComponent, ComponentInit>(OnStationNameSetupInit);
    }

    private void OnStationNameSetupInit(EntityUid uid, StationNameSetupComponent component, ComponentInit args)
    {
        if (!HasComp<StationDataComponent>(uid))
            return;

        _station.RenameStation(uid, GenerateStationName(component), false);
    }

    /// <summary>
    /// Generates a station name from the given config.
    /// </summary>
    private string GenerateStationName(StationNameSetupComponent config)
    {
        // Fire edit start - локализуемые шаблоны названий станции
        var template = ResolveStationNameTemplate(
            config.StationNameTemplate,
            config.StationNameTemplateLocId,
            _localization);
        return config.NameGenerator is not null
            ? config.NameGenerator.FormatName(template)
            : template;
        // Fire edit end
    }
}
