#nullable enable

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Content.Client._Scp.Decals;
using Content.Client._Scp.UI;
using Content.Server.Silicons.Laws;
using Content.Server.Station.Components;
using Content.Server.Station.Systems;
using Content.Shared._Scp.Guidebook;
using Content.Shared._Sunrise.InteractionsPanel.Data.Conditions;
using Content.Shared._Sunrise.InteractionsPanel.Data.Prototypes;
using Content.Shared.Decals;
using Content.Shared.Dataset;
using Content.Shared.Localizations;
using Content.Shared.Maps;
using Content.Shared.Materials;
using Content.Shared.Station;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.ContentPack;
using Robust.Shared.ColorNaming;
using Robust.Shared.GameObjects;
using Robust.Shared.Localization;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.IntegrationTests.Tests._Scp.Localization;

[TestFixture]
public sealed class SpanishLocalizationTest
{
    private const string ScpComplexCompactMap = "ScpComplexCompact";
    private const string AdminFaxBaseMap = "AdminFaxBase";

    [Test]
    public async Task StandardInteractionsResolveInSpanish()
    {
        await using var pair = await PoolManager.GetServerClient();
        var names = new List<(string Id, string Value)>();
        var messages = new List<(string Id, string Value)>();
        var bodyAreaCategories = new HashSet<string>();
        string? cultureName = null;
        var interactionCount = 0;

        await pair.Server.WaitAssertion(() =>
        {
            var localization = pair.Server.ResolveDependency<ILocalizationManager>();
            var interactions = pair.Server.ProtoMan.EnumeratePrototypes<InteractionPrototype>().ToList();

            cultureName = localization.DefaultCulture?.Name;
            interactionCount = interactions.Count;

            foreach (var interaction in interactions)
            {
                names.Add((interaction.ID, localization.GetString(interaction.Name)));
                messages.AddRange(interaction.InteractionMessages.Select(message =>
                    (interaction.ID, localization.GetString(message))));

                foreach (var condition in interaction.AppearConditions.OfType<BodyAreaTagCondition>())
                {
                    bodyAreaCategories.UnionWith(condition.Categories);
                }
            }
        });

        await pair.CleanReturnAsync();

        Assert.Multiple(() =>
        {
            Assert.That(cultureName, Is.EqualTo("es-ES"));
            Assert.That(interactionCount, Is.EqualTo(37));
            Assert.That(messages, Has.Count.EqualTo(91));
            Assert.That(typeof(InteractionPrototype).GetProperty(nameof(InteractionPrototype.Name))!.PropertyType,
                Is.EqualTo(typeof(LocId)));
            Assert.That(typeof(InteractionPrototype).GetProperty(nameof(InteractionPrototype.InteractionMessages))!.PropertyType,
                Is.EqualTo(typeof(List<LocId>)));

            foreach (var (id, value) in names.Concat(messages))
            {
                Assert.That(value, Is.Not.Empty, id);
                Assert.That(value, Does.Not.Contain("interaction-name-").And.Not.Contain("interaction-message-"), id);
                Assert.That(value, Does.Not.Contain("Unknown messageId"), id);
                Assert.That(value.Any(character => character is >= '\u0400' and <= '\u04FF'), Is.False, id);
            }

            Assert.That(bodyAreaCategories, Is.Not.Empty);
            Assert.That(bodyAreaCategories.All(category => category.All(character =>
                character is >= 'a' and <= 'z' or '-')), Is.True);
        });
    }

    [Test]
    public async Task IonStormDatasetsResolveInSpanish()
    {
        await using var pair = await PoolManager.GetServerClient();
        var datasetCount = 0;
        var valueCount = 0;
        var sourceDatasetCount = 0;
        var sourceValueCount = 0;
        var missingMessages = new List<string>();
        var mismatchedDatasets = new List<string>();
        string? cultureName = null;
        string? annoyingAdjective = null;

        await pair.Server.WaitAssertion(() =>
        {
            var localization = pair.Server.ResolveDependency<ILocalizationManager>();
            var datasets = pair.Server.ProtoMan
                .EnumeratePrototypes<LocalizedDatasetPrototype>()
                .Where(prototype => prototype.ID.StartsWith("IonStorm"))
                .ToList();
            var sourceDatasets = pair.Server.ProtoMan
                .EnumeratePrototypes<DatasetPrototype>()
                .Where(prototype => prototype.ID.StartsWith("IonStorm"))
                .ToDictionary(prototype => prototype.ID);

            cultureName = localization.DefaultCulture?.Name;
            datasetCount = datasets.Count;
            valueCount = datasets.Sum(prototype => prototype.Values.Count);
            sourceDatasetCount = sourceDatasets.Count;
            sourceValueCount = sourceDatasets.Values.Sum(prototype => prototype.Values.Count);

            foreach (var dataset in datasets)
            {
                if (!sourceDatasets.TryGetValue(dataset.ID, out var source) ||
                    source.Values.Count != dataset.Values.Count)
                {
                    mismatchedDatasets.Add(dataset.ID);
                }
            }

            foreach (var locId in datasets.SelectMany(prototype => prototype.Values))
            {
                if (!localization.HasString(locId))
                    missingMessages.Add(locId);
            }

            var adjectives = datasets.SingleOrDefault(prototype => prototype.ID == "IonStormAdjectives");
            if (adjectives is not null)
                annoyingAdjective = localization.GetString(adjectives.Values[1]);
        });

        await pair.CleanReturnAsync();

        Assert.Multiple(() =>
        {
            Assert.That(cultureName, Is.EqualTo("es-ES"));
            Assert.That(datasetCount, Is.EqualTo(18));
            Assert.That(valueCount, Is.EqualTo(1017));
            Assert.That(sourceDatasetCount, Is.EqualTo(datasetCount));
            Assert.That(sourceValueCount, Is.EqualTo(valueCount));
            Assert.That(mismatchedDatasets, Is.Empty);
            Assert.That(missingMessages, Is.Empty);
            Assert.That(annoyingAdjective, Is.EqualTo("MOLESTO"));
        });
    }

    [Test]
    public async Task IonStormGeneratesEveryLawBranchInSpanish()
    {
        await using var pair = await PoolManager.GetServerClient();
        var generated = new List<(int Branch, int Subject, bool Part, string Law)>();
        var singularFeelings = new HashSet<string>();
        var pluralFeelings = new HashSet<string>();
        string? cultureName = null;

        await pair.Server.WaitAssertion(() =>
        {
            var localization = pair.Server.ResolveDependency<ILocalizationManager>();
            var system = pair.Server.System<IonStormSystem>();
            var generateLaw = typeof(IonStormSystem).GetMethod(
                "GenerateLaw",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null,
                [typeof(int?), typeof(int?), typeof(bool?)],
                null);

            Assert.That(generateLaw, Is.Not.Null);
            cultureName = localization.DefaultCulture?.Name;

            var feelings = pair.Server.ProtoMan.Index<LocalizedDatasetPrototype>("IonStormFeelings");
            var feelingsPlural = pair.Server.ProtoMan.Index<LocalizedDatasetPrototype>("IonStormFeelingsPlural");
            singularFeelings.UnionWith(feelings.Values.Select(localization.GetString));
            pluralFeelings.UnionWith(feelingsPlural.Values.Select(localization.GetString));

            for (var branch = 0; branch < 35; branch++)
            {
                for (var subject = 0; subject < 5; subject++)
                {
                    foreach (var part in new[] { false, true })
                    {
                        var law = (string) generateLaw!.Invoke(system, [branch, subject, part])!;
                        generated.Add((branch, subject, part, law));
                    }
                }
            }
        });

        await pair.CleanReturnAsync();

        Assert.Multiple(() =>
        {
            Assert.That(cultureName, Is.EqualTo("es-ES"));
            Assert.That(generated, Has.Count.EqualTo(350));
            Assert.That(generated.Select(entry => entry.Branch).Distinct().Count(), Is.EqualTo(35));

            foreach (var entry in generated)
            {
                Assert.That(entry.Law, Is.Not.Empty, $"Rama {entry.Branch}, sujeto {entry.Subject}");
                Assert.That(entry.Law, Does.Not.Contain("ion-storm-"), $"Rama {entry.Branch}, sujeto {entry.Subject}");
                Assert.That(entry.Law, Does.Not.Contain("Unknown messageId"), $"Rama {entry.Branch}, sujeto {entry.Subject}");
                Assert.That(entry.Law, Does.Not.Contain("{").And.Not.Contain("}"), $"Rama {entry.Branch}, sujeto {entry.Subject}");
                Assert.That(entry.Law.Any(character => character is >= '\u0400' and <= '\u04FF'), Is.False,
                    $"Rama {entry.Branch}, sujeto {entry.Subject}");
            }

            foreach (var branch in Enumerable.Range(26, 6))
            {
                Assert.That(generated.Single(entry => entry.Branch == branch && entry.Subject == 0 && entry.Part).Law,
                    Does.Contain("FORMAN PARTE DE LA TRIPULACIÓN").And.Not.Contain("NO FORMAN PARTE"));
                Assert.That(generated.Single(entry => entry.Branch == branch && entry.Subject == 0 && !entry.Part).Law,
                    Does.Contain("NO FORMAN PARTE DE LA TRIPULACIÓN"));
            }

            foreach (var branch in new[] { 13, 14 })
            {
                for (var subject = 0; subject < 5; subject++)
                {
                    var law = generated.Single(entry => entry.Branch == branch && entry.Subject == subject && entry.Part).Law;
                    var expectedFeelings = subject is 0 or 3 ? pluralFeelings : singularFeelings;
                    Assert.That(expectedFeelings.Any(law.Contains), Is.True,
                        $"La rama {branch} no usó la conjugación esperada para el sujeto {subject}: {law}");
                }
            }
        });
    }

    [Test]
    public async Task RegionalManagementGridAppliesLocalizedName()
    {
        await using var pair = await PoolManager.GetServerClient(new PoolSettings { Dirty = true });
        string? gridName = null;
        string? mapSource = null;

        await pair.Server.WaitPost(() =>
        {
            var componentFactory = pair.Server.ResolveDependency<IComponentFactory>();
            Assert.That(componentFactory.TryGetRegistration("LocalizedGridName", out _), Is.True);

            var localizedName = componentFactory.GetComponent("LocalizedGridName");
            var nameField = localizedName.GetType().GetField("Name");
            Assert.That(nameField, Is.Not.Null);
            nameField!.SetValue(localizedName, new LocId("grid-name-scp-command-offices"));

            var entity = pair.Server.EntMan.SpawnEntity(null, MapCoordinates.Nullspace);
            pair.Server.System<MetaDataSystem>().SetEntityName(entity, "Command Offices");
            pair.Server.EntMan.AddComponent(entity, localizedName);
            gridName = pair.Server.MetaData(entity).EntityName;
            pair.Server.EntMan.DeleteEntity(entity);

            var resources = pair.Server.ResolveDependency<IResourceManager>();
            using var stream = resources.ContentFileRead(
                new ResPath("/Maps/_Scp/Events/RegionalManagement.yml"));
            using var reader = new StreamReader(stream);
            mapSource = reader.ReadToEnd();
        });

        await pair.CleanReturnAsync();

        Assert.Multiple(() =>
        {
            Assert.That(gridName, Is.EqualTo("Oficinas del Mando"));
            Assert.That(mapSource, Does.Contain("- type: LocalizedGridName"));
            Assert.That(mapSource, Does.Contain("name: grid-name-scp-command-offices"));
        });
    }

    [Test]
    public async Task PresentationControlsPreserveAlreadyLocalizedText()
    {
        await using var pair = await PoolManager.GetServerClient();
        string? coloredName = null;
        string? coloredDescription = null;

        await pair.Client.WaitAssertion(() =>
        {
            const string renderedText = "generic-unknown";
            var coloredInfo = new ColoredInfo
            {
                NameString = renderedText,
                DescriptionString = renderedText,
            };

            coloredName = FindDescendant<Label>(coloredInfo).Text;
            coloredDescription = FindDescendant<RichTextLabel>(coloredInfo).GetMessage();
        });

        await pair.CleanReturnAsync();

        Assert.Multiple(() =>
        {
            Assert.That(coloredName, Is.EqualTo("generic-unknown"));
            Assert.That(coloredDescription, Is.EqualTo("generic-unknown"));
        });
    }

    private static T FindDescendant<T>(Control root) where T : Control
    {
        foreach (var child in root.Children)
        {
            if (child is T match)
                return match;

            try
            {
                return FindDescendant<T>(child);
            }
            catch (InvalidOperationException)
            {
                // Sigue buscando en las demás ramas del árbol de controles.
            }
        }

        throw new InvalidOperationException($"No se encontró un control {typeof(T).Name}.");
    }

    [Test]
    public async Task MapNameCanBeReadWithoutIoCContext()
    {
        await using var pair = await PoolManager.GetServerClient();
        GameMapPrototype? map = null;
        string? mapName = null;
        Exception? exception = null;

        await pair.Server.WaitAssertion(() =>
        {
            map = pair.Server.ProtoMan.Index<GameMapPrototype>(ScpComplexCompactMap);
        });

        try
        {
            mapName = await Task.Run(() => map!.MapName);
        }
        catch (Exception caught)
        {
            exception = caught;
        }

        await pair.CleanReturnAsync();

        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Null);
            Assert.That(mapName, Is.EqualTo("Compact-type complex"));
        });
    }

    [Test]
    public async Task StationNameSystemAppliesLocalizedTemplate()
    {
        await using var pair = await PoolManager.GetServerClient(new PoolSettings { Dirty = true });
        string? stationName = null;

        await pair.Server.WaitAssertion(() =>
        {
            var stationSystem = pair.Server.System<StationSystem>();
            var map = pair.Server.ProtoMan.Index<GameMapPrototype>(ScpComplexCompactMap);
            var sourceConfig = map.Stations["ScpComplexCompact"];
            var componentName = pair.Server.ResolveDependency<IComponentFactory>()
                .GetComponentName<StationNameSetupComponent>();
            var testConfig = new StationConfig
            {
                StationPrototype = "StandardStationArena",
                StationComponentOverrides = new ComponentRegistry
                {
                    [componentName] = sourceConfig.StationComponentOverrides[componentName],
                },
            };
            var station = stationSystem.InitializeNewStation(testConfig, null);
            stationName = pair.Server.MetaData(station).EntityName;

            stationSystem.DeleteStation(station);
        });

        await pair.Server.WaitRunTicks(1);
        await pair.CleanReturnAsync();

        Assert.That(stationName, Does.Match("^Sitio-[0-9]{2}, tipo Compact$"));
    }

    [Test]
    public async Task SunriseStationTemplatesResolveInSpanish()
    {
        var expectedTemplates = new Dictionary<ProtoId<GameMapPrototype>, string>
        {
            ["SunriseBagel"] = "{0} Bagel {1}",
            ["SunriseBox"] = "{0} Box {1}",
            ["SunriseConvex"] = "{0} Convex {1}",
            ["SunriseDelta"] = "{0} Delta {1}",
            ["SunriseMarathon"] = "{0} Marathon {1}",
            ["SunrisePacked"] = "{0} Packed {1}",
            ["PlanetPrisonOld"] = "{0} Prisión planetaria Nox {1}",
        };

        await using var pair = await PoolManager.GetServerClient();

        await pair.Server.WaitAssertion(() =>
        {
            var componentName = pair.Server.ResolveDependency<IComponentFactory>()
                .GetComponentName<StationNameSetupComponent>();
            var localization = pair.Server.ResolveDependency<ILocalizationManager>();

            foreach (var (mapId, expectedTemplate) in expectedTemplates)
            {
                var map = pair.Server.ProtoMan.Index(mapId);
                var station = map.Stations.Values.Single();
                station.StationComponentOverrides.TryGetComponent(componentName, out var stationComponent);
                var stationNameSetup = (StationNameSetupComponent) stationComponent!;
                var localizedTemplate = StationNameSystem.ResolveStationNameTemplate(
                    stationNameSetup.StationNameTemplate,
                    stationNameSetup.StationNameTemplateLocId,
                    localization);

                Assert.That(localizedTemplate, Is.EqualTo(expectedTemplate), mapId.Id);
            }
        });

        await pair.CleanReturnAsync();
    }

    [Test]
    public async Task SalvageTicketMaterialUsesDistinctLocalizedNameAndUnit()
    {
        await using var pair = await PoolManager.GetServerClient();

        await pair.Server.WaitAssertion(() =>
        {
            var localization = pair.Server.ResolveDependency<ILocalizationManager>();
            var material = pair.Server.ProtoMan.Index<MaterialPrototype>("SalvageTicket");

            Assert.Multiple(() =>
            {
                Assert.That(material.Name, Is.EqualTo("materials-salvage-ticket"));
                Assert.That(material.Unit, Is.EqualTo(new LocId("materials-unit-tickets")));
                Assert.That(localization.GetString(material.Name), Is.EqualTo("vale de expedición"));
                Assert.That(localization.GetString(material.Unit), Is.EqualTo("vale de expedición"));
            });
        });

        await pair.CleanReturnAsync();
    }

    [Test]
    public async Task DecalPlacerMetadataResolvesForEveryPrototype()
    {
        await using var pair = await PoolManager.GetServerClient();
        var missingMessages = new List<string>();
        var decalCount = 0;
        var visibleDecalCount = 0;
        var paletteCount = 0;
        var categoryCount = 0;
        var colorCount = 0;
        string? whiteColorName = null;
        string? formattedColorName = null;
        HashSet<string>? englishCatalogIds = null;
        HashSet<string>? spanishCatalogIds = null;

        await pair.Client.WaitAssertion(() =>
        {
            const string missingFallback = "\u0000missing";
            var localization = pair.Client.ResolveDependency<ILocalizationManager>();
            var prototypes = pair.Client.ResolveDependency<IPrototypeManager>();
            var categories = new HashSet<string>();
            var colors = new HashSet<string>();
            whiteColorName = ColorNaming.Describe(Color.White, localization);
            formattedColorName = localization.GetString(
                "color-hue-chroma-lightness",
                ("hue", "rojo"),
                ("chroma", "intenso"),
                ("lightness", "claro"));

            foreach (var decal in prototypes.EnumeratePrototypes<DecalPrototype>())
            {
                decalCount++;
                if (decal.ShowMenu)
                    visibleDecalCount++;

                categories.UnionWith(decal.Tags);

                if (FireDecalLocalization.GetDecalName(
                        localization,
                        decal.ID,
                        missingFallback) == missingFallback)
                {
                    missingMessages.Add($"decal-name:{decal.ID}");
                }
            }

            foreach (var category in categories)
            {
                if (FireDecalLocalization.GetCategoryName(
                        localization,
                        category,
                        missingFallback) == missingFallback)
                {
                    missingMessages.Add($"decal-category:{category}");
                }
            }

            foreach (var palette in prototypes.EnumeratePrototypes<ColorPalettePrototype>())
            {
                paletteCount++;
                colors.UnionWith(palette.Colors.Keys);

                if (FireDecalLocalization.GetPaletteName(
                        localization,
                        palette.ID,
                        missingFallback) == missingFallback)
                {
                    missingMessages.Add($"decal-palette:{palette.ID}");
                }
            }

            foreach (var color in colors)
            {
                if (FireDecalLocalization.GetColorName(
                        localization,
                        color,
                        missingFallback) == missingFallback)
                {
                    missingMessages.Add($"decal-color:{color}");
                }
            }

            categoryCount = categories.Count;
            colorCount = colors.Count;

            var resources = pair.Client.ResolveDependency<IResourceManager>();
            englishCatalogIds = ReadFluentMessageIds(
                resources,
                new ResPath("/Locale/en-US/_strings/decals/decal-catalog.ftl"));
            spanishCatalogIds = ReadFluentMessageIds(
                resources,
                new ResPath("/Locale/es-ES/_strings/decals/decal-catalog.ftl"));
        });

        await pair.CleanReturnAsync();

        Assert.Multiple(() =>
        {
            Assert.That(decalCount, Is.EqualTo(1168));
            Assert.That(visibleDecalCount, Is.EqualTo(1156));
            Assert.That(paletteCount, Is.EqualTo(11));
            Assert.That(categoryCount, Is.EqualTo(17));
            Assert.That(colorCount, Is.EqualTo(145));
            Assert.That(whiteColorName, Is.EqualTo("blanco"));
            Assert.That(formattedColorName, Is.EqualTo("rojo claro e intenso"));
            Assert.That(spanishCatalogIds, Is.EqualTo(englishCatalogIds));
            Assert.That(spanishCatalogIds, Has.Count.EqualTo(1341));
            Assert.That(spanishCatalogIds!.Count(id => id.StartsWith("decal-name-")), Is.EqualTo(1168));
            Assert.That(spanishCatalogIds.Count(id => id.StartsWith("decal-category-")), Is.EqualTo(17));
            Assert.That(spanishCatalogIds.Count(id => id.StartsWith("decal-palette-")), Is.EqualTo(11));
            Assert.That(spanishCatalogIds.Count(id => id.StartsWith("decal-color-")), Is.EqualTo(145));
            Assert.That(
                missingMessages,
                Is.Empty,
                $"Missing Decal Placer localization: {string.Join(", ", missingMessages)}");
        });
    }

    private static HashSet<string> ReadFluentMessageIds(IResourceManager resources, ResPath path)
    {
        using var stream = resources.ContentFileRead(path);
        using var reader = new StreamReader(stream);
        var result = new HashSet<string>();

        while (reader.ReadLine() is { } line)
        {
            if (line.Length == 0 || char.IsWhiteSpace(line[0]) || line[0] == '#')
                continue;

            var separator = line.IndexOf('=');
            if (separator <= 0)
                continue;

            result.Add(line[..separator].TrimEnd());
        }

        return result;
    }

    [Test]
    public async Task StationNameTemplateFallsBackWhenLocIdIsMissing()
    {
        await using var pair = await PoolManager.GetServerClient();

        await pair.Server.WaitAssertion(() =>
        {
            var localization = pair.Server.ResolveDependency<ILocalizationManager>();
            var template = StationNameSystem.ResolveStationNameTemplate(
                "Foundation Complex {0}",
                new LocId("missing-station-name-template"),
                localization);

            Assert.That(template, Is.EqualTo("Foundation Complex {0}"));
        });

        await pair.CleanReturnAsync();
    }

    [Test]
    public async Task SpanishCultureLoadsWithEnglishFallbackFunctions()
    {
        await using var pair = await PoolManager.GetServerClient();

        var localization = pair.Server.ResolveDependency<ILocalizationManager>();
        string? cultureName = null;
        string? fallbackMessage = null;
        string? fallbackFunctionMessage = null;
        string? grammaticalMessage = null;
        string? pluralMessage = null;
        string? localizedGuidebookMessage = null;
        string? spanishMapName = null;
        string? englishMapName = null;
        string? spanishStationTemplate = null;
        string? englishStationTemplate = null;
        string? spanishStationName = null;
        string? spanishAdministrationName = null;
        string? englishAdministrationName = null;
        string? englishVpnMessage = null;
        string? englishStatsCharacter = null;
        string? englishStatsCreamed = null;
        ResPath? localizedGuidebookPath = null;
        Dictionary<string, string>? centralMessages = null;

        await pair.Server.WaitPost(() =>
        {
            var prototypes = pair.Server.ResolveDependency<IPrototypeManager>();
            var componentFactory = pair.Server.ResolveDependency<IComponentFactory>();
            cultureName = localization.DefaultCulture?.Name;
            fallbackMessage = localization.GetString("absinthe-effect-hear-voice");
            fallbackFunctionMessage = localization.GetString("power-monitoring-window-value", ("value", 1000));
            grammaticalMessage = localization.GetString("petting-success-cat", ("target", "gato"));
            pluralMessage = localization.GetString(
                "lathe-menu-material-amount",
                ("amount", 2),
                ("unit", "lámina"));
            localizedGuidebookMessage = localization.GetString(
                "entity-effect-guidebook-knockdown",
                ("type", "add"),
                ("chance", 1),
                ("key", "unused"),
                ("time", 2));
            centralMessages = new Dictionary<string, string>
            {
                ["admin"] = localization.GetString("admin-manager-admin-login-message", ("name", "Hermes")),
                ["rules"] = localization.GetString("starting-rule-selected-preset", ("preset", "extended")),
                ["station"] = localization.GetString("job-greet-station-name", ("stationName", "Dev")),
                ["job"] = localization.GetString("job-greet-introduce-job-name", ("jobName", "Director del complejo")),
                ["tutorial"] = localization.GetString("ui-tutorial"),
                ["changelog"] = localization.GetString("changelog-button-new-entries"),
                ["roadmap"] = localization.GetString("server-info-roadmap-button"),
                ["roadmapHeader"] = localization.GetString("ui-roadmap-header"),
                ["roadmapPlanned"] = localization.GetString("ui-roadmap-state-planned"),
                ["roadmapDetails"] = localization.GetString("ui-roadmap-click-for-details"),
                ["profileAccount"] = localization.GetString("user-profile-account-title"),
                ["profileBindings"] = localization.GetString("user-profile-bindings-title"),
                ["profileUnavailable"] = localization.GetString("user-profile-binding-unavailable"),
                ["profileSponsor"] = localization.GetString("user-profile-sponsor-title"),
                ["profileNoSponsor"] = localization.GetString("user-profile-no-sponsor"),
                ["profileLevel"] = localization.GetString("user-profile-level"),
                ["profileInfo"] = localization.GetString("user-profile-sponsor-info-button"),
                ["profileBuy"] = localization.GetString("user-profile-sponsor-buy-button"),
                ["contributors"] = localization.GetString("contributors-column-contributions"),
                ["pet"] = localization.GetString("pet-selection-select-lobby"),
                ["mentor"] = localization.GetString("ui-options-function-open-mentor-help"),
                ["playerCount"] = localization.GetString("player-tab-player-count", ("count", 5)),
                ["infoTitle"] = localization.GetString("ui-info-title"),
                ["rulesHeader"] = localization.GetString("ui-rules-header"),
                ["voteTitle"] = localization.GetString("ui-vote-create-title"),
                ["observeWarning"] = localization.GetString("observe-warning-window-title"),
                ["mapText"] = localization.GetString("map-text-dev-department-other"),
                ["voteRequirement"] = localization.GetString(
                    "ui-vote-trusted-users-notice-time-req",
                    ("timeReq", 1),
                    ("ghostTimeReq", 2)),
                ["zombieAccent"] = localization.GetString("ent-StatusEffectZombieAccent"),
                ["concrete"] = localization.GetString("materials-concrete"),
                ["bags"] = localization.GetString(
                    "lathe-menu-material-amount",
                    ("amount", 2),
                    ("unit", localization.GetString("materials-unit-bag"))),
                ["tickets"] = localization.GetString(
                    "lathe-menu-material-amount",
                    ("amount", 2),
                    ("unit", localization.GetString("materials-unit-tickets"))),
                ["stack"] = localization.GetString("stack-space-carp-tooth", ("amount", 2)),
                ["plushieTag"] = localization.GetString("construction-graph-tag-plushiekalium"),
                ["scannerTag"] = localization.GetString("construction-graph-tag-trayscanner"),
                ["jawsTag"] = localization.GetString("construction-graph-tag-jawsoflife"),
                ["drillTag"] = localization.GetString("construction-graph-tag-powerdrill"),
                ["fallbackReference"] = localization.GetString("ent-APCBasic"),
                ["cableStack"] = localization.GetString("ent-CableDetStack10"),
                ["yakuzaShoes"] = localization.GetString("ent-ClothingShoesDameDane"),
                ["yellowBandanaHead"] = localization.GetString("ent-ClothingHeadBandYellow"),
                ["yellowBandanaMask"] = localization.GetString("ent-ClothingMaskBandYellow"),
                ["combatModeAction"] = localization.GetString("ent-ActionCombatModeToggle"),
                ["xenoResinWindow"] = localization.GetString("ent-XenoResinWindow"),
                ["shadyBox"] = localization.GetString("ent-PresentRandomPrisoner"),
                ["decalPlacer"] = localization.GetString("decal-placer-window-title"),
                ["tutorialExamine"] = localization.GetString("intro-tut-examine-airlock-bubble"),
                ["roboticsBrain"] = localization.GetString("robotics-console-brain", ("brain", true)),
                ["doubleStunbaton"] = localization.GetString("ent-DoubleStunbaton"),
                ["usspCaptain"] = localization.GetString("job-name-ussp-captain"),
                ["formattedUsspCaptain"] = ContentLocalizationManager.FormatTitleCase(
                    localization.GetString("job-name-ussp-captain")),
                ["baseTutorials"] = localization.GetString("base-tutorials"),
                ["introductionTutorialName"] = localization.GetString("introduction-tutorial-name"),
                ["introductionTutorialTooltip"] = localization.GetString("introduction-tutorial-tooltip"),
                ["guideException32"] = localization.GetString("guide-entry-sr-rule-excep-3-2"),
                ["guideException34"] = localization.GetString("guide-entry-sr-rule-excep-3-4"),
                ["guideException37"] = localization.GetString("guide-entry-sr-rule-excep-3-7"),
                ["speciesDemon"] = localization.GetString("species-name-demon"),
                ["speciesMilira"] = localization.GetString("species-name-milira"),
                ["lobbyAnimationScreen"] = localization.GetString("lobby-animation-Screen"),
                ["lobbyAnimationScp173"] = localization.GetString("lobby-animation-Scp173"),
                ["lobbyAnimationMtf"] = localization.GetString("lobby-animation-Mtf"),
                ["lobbyAnimationPc"] = localization.GetString("lobby-animation-PC"),
                ["lobbyAnimationCamera"] = localization.GetString("lobby-animation-Camera"),
                ["lobbyAnimationLogo"] = localization.GetString("lobby-animation-Logo"),
                ["lobbyAnimationRedGuy"] = localization.GetString("lobby-animation-RedGuy"),
                ["lobbyAnimationScp049"] = localization.GetString("lobby-animation-Scp049"),
                ["lobbyAnimationScp096"] = localization.GetString("lobby-animation-Scp096"),
                ["lobbyAnimationFoundation"] = localization.GetString("lobby-animation-Foundation"),
                ["lobbyAnimationWorldMap"] = localization.GetString("lobby-animation-WorldMap"),
                ["lobbyAnimationDeepFacility"] = localization.GetString("lobby-animation-DeepFacility"),
                ["gameModeTie"] = localization.GetString("ui-vote-gamemode-tie", ("picked", "Narrador")),
                ["gameModeWin"] = localization.GetString("ui-vote-gamemode-win", ("winner", "Narrador")),
                ["mapTie"] = localization.GetString("ui-vote-map-tie", ("picked", "Complejo Compact")),
                ["mapWin"] = localization.GetString("ui-vote-map-win", ("winner", "Complejo Compact")),
                ["scp049CannotZombify"] = localization.GetString("scp049-cannot-zombify-entity", ("name", "Sujeto")),
                ["areaReaction"] = localization.GetString(
                    "entity-effect-guidebook-area-reaction",
                    ("chance", 1),
                    ("duration", 2)),
                ["knockdownUpdate"] = localization.GetString(
                    "entity-effect-guidebook-knockdown",
                    ("chance", 1),
                    ("key", "entity-effect-status-effect-KnockedDown"),
                    ("type", "update"),
                    ("time", 2)),
                ["commandOfficesGrid"] = localization.GetString("grid-name-scp-command-offices"),
                ["transitHubGrid"] = localization.GetString("map-name-scp-transit-hub"),
                ["emergencyShuttleGrid"] = localization.GetString("grid-name-scp-armored-emergency-shuttle"),
            };

            var originalCulture = localization.DefaultCulture!;
            var map = prototypes.Index<GameMapPrototype>(ScpComplexCompactMap);
            var stationNameComponent = componentFactory.GetComponentName<StationNameSetupComponent>();
            var station = map.Stations["ScpComplexCompact"];
            station.StationComponentOverrides.TryGetComponent(stationNameComponent, out var stationComponent);
            var stationNameSetup = (StationNameSetupComponent) stationComponent!;
            var administrationMap = prototypes.Index<GameMapPrototype>(AdminFaxBaseMap);
            var administration = administrationMap.Stations["AdminFaxBase"];
            administration.StationComponentOverrides.TryGetComponent(stationNameComponent, out var administrationComponent);
            var administrationNameSetup = (StationNameSetupComponent) administrationComponent!;

            spanishMapName = map.GetLocalizedName(localization);
            spanishStationTemplate = localization.GetString(stationNameSetup.StationNameTemplateLocId!.Value);
            spanishStationName = stationNameSetup.NameGenerator!.FormatName(spanishStationTemplate);
            spanishAdministrationName = administrationNameSetup.NameGenerator!.FormatName(
                localization.GetString(administrationNameSetup.StationNameTemplateLocId!.Value));
            try
            {
                localization.SetCulture(CultureInfo.GetCultureInfo("en-US"));
                englishMapName = map.GetLocalizedName(localization);
                englishStationTemplate = localization.GetString(stationNameSetup.StationNameTemplateLocId!.Value);
                englishAdministrationName = administrationNameSetup.NameGenerator!.FormatName(
                    localization.GetString(administrationNameSetup.StationNameTemplateLocId!.Value));
                englishVpnMessage = localization.GetString("panic-bunker-account-reason-vpn");
                englishStatsCharacter = localization.GetString("statsentry-character");
                englishStatsCreamed = localization.GetString("statsentry-total-creampied", ("total", 3));
            }
            finally
            {
                localization.SetCulture(originalCulture);
            }
        });

        await pair.Client.WaitPost(() =>
        {
            var resources = pair.Client.ResolveDependency<IResourceManager>();
            localizedGuidebookPath = GuidebookLocalization.GetDocumentPath(
                new ResPath("/ServerInfo/_Scp/Guidebook/Scp082/Containment.xml"),
                CultureInfo.GetCultureInfo("es-ES"),
                resources.ContentFileExists);
        });

        await pair.CleanReturnAsync();

        Assert.Multiple(() =>
        {
            Assert.That(cultureName, Is.EqualTo("es-ES"));
            Assert.That(fallbackMessage, Is.EqualTo("You hear a tiny voice. \"Tee hee hee!\""));
            Assert.That(fallbackFunctionMessage, Is.EqualTo("1000,0 W"));
            Assert.That(grammaticalMessage, Is.EqualTo("Acaricias al gato en la cabecita peluda."));
            Assert.That(pluralMessage, Does.Contain("2 láminas"));
            Assert.That(localizedGuidebookMessage, Does.Contain("2 segundos"));
            Assert.That(spanishMapName, Is.EqualTo("Complejo tipo Compact"));
            Assert.That(englishMapName, Is.EqualTo("Compact-type complex"));
            Assert.That(spanishStationTemplate, Is.EqualTo("Sitio-{0}, tipo Compact"));
            Assert.That(englishStationTemplate, Is.EqualTo("Site-{0}, Compact type"));
            Assert.That(spanishStationName, Does.Match("^Sitio-[0-9]{2}, tipo Compact$"));
            Assert.That(spanishAdministrationName, Is.EqualTo("Base de la Administración Regional"));
            Assert.That(englishAdministrationName, Is.EqualTo("Regional Administration Base"));
            Assert.That(englishVpnMessage, Is.EqualTo("The server does not allow access via VPN connections."));
            Assert.That(englishStatsCharacter, Is.EqualTo("Character:"));
            Assert.That(englishStatsCreamed, Is.EqualTo("Players received 3 cream pies to the face."));
            Assert.That(
                localizedGuidebookPath,
                Is.EqualTo(new ResPath("/ServerInfo/es-ES/_Scp/Guidebook/Scp082/Containment.xml")));
            Assert.That(centralMessages, Is.Not.Null);
            Assert.That(centralMessages!["admin"], Is.EqualTo("Inicio de sesión de administrador: Hermes"));
            Assert.That(centralMessages["rules"], Is.EqualTo("Reglas de juego en uso: extended"));
            Assert.That(centralMessages["station"], Is.EqualTo("Te damos la bienvenida a bordo de Dev."));
            Assert.That(centralMessages["job"], Is.EqualTo("Tu puesto es: Director del complejo."));
            Assert.That(centralMessages["tutorial"], Is.EqualTo("Tutorial"));
            Assert.That(centralMessages["changelog"], Is.EqualTo("Registro de cambios (¡novedades!)"));
            Assert.That(centralMessages["roadmap"], Is.EqualTo("Hoja de ruta"));
            Assert.That(centralMessages["roadmapHeader"], Is.EqualTo("PLAN DE DESARROLLO"));
            Assert.That(centralMessages["roadmapPlanned"], Is.EqualTo("PREVISTO"));
            Assert.That(centralMessages["roadmapDetails"], Is.EqualTo("Haz clic para ver los detalles"));
            Assert.That(centralMessages["profileAccount"], Is.EqualTo("Cuenta de Makura Games ID"));
            Assert.That(centralMessages["profileBindings"], Is.EqualTo("Vinculaciones"));
            Assert.That(centralMessages["profileUnavailable"], Is.EqualTo("No disponible temporalmente"));
            Assert.That(centralMessages["profileSponsor"], Is.EqualTo("Patrocinio"));
            Assert.That(centralMessages["profileNoSponsor"], Is.EqualTo("Sin patrocinio"));
            Assert.That(centralMessages["profileLevel"], Is.EqualTo("Nivel:"));
            Assert.That(centralMessages["profileInfo"], Is.EqualTo("Información"));
            Assert.That(centralMessages["profileBuy"], Is.EqualTo("Adquirir"));
            Assert.That(centralMessages["contributors"], Is.EqualTo("Contribuciones"));
            Assert.That(centralMessages["pet"], Is.EqualTo("Cambiar mascota"));
            Assert.That(centralMessages["mentor"], Is.EqualTo("Abrir la ayuda de mentor"));
            Assert.That(centralMessages["playerCount"], Is.EqualTo("Jugadores: 5"));
            Assert.That(centralMessages["infoTitle"], Is.EqualTo("Información"));
            Assert.That(centralMessages["rulesHeader"], Is.EqualTo("Reglas del servidor"));
            Assert.That(centralMessages["voteTitle"], Is.EqualTo("Convocar una votación"));
            Assert.That(centralMessages["observeWarning"], Is.EqualTo("Advertencia"));
            Assert.That(centralMessages["mapText"], Is.EqualTo("Otros"));
            Assert.That(centralMessages["voteRequirement"], Does.Contain("1 hora"));
            Assert.That(centralMessages["voteRequirement"], Does.Contain("2 segundos"));
            Assert.That(centralMessages["zombieAccent"], Is.EqualTo("acento zombi"));
            Assert.That(centralMessages["concrete"], Is.EqualTo("cemento"));
            Assert.That(centralMessages["bags"], Does.Contain("2 sacos"));
            Assert.That(centralMessages["tickets"], Does.Contain("2 vales de expedición"));
            Assert.That(centralMessages["stack"], Is.EqualTo("dientes de carpa espacial"));
            Assert.That(centralMessages["plushieTag"], Is.EqualTo("Kalium Fon Dez"));
            Assert.That(centralMessages["scannerTag"], Is.EqualTo("escáner de rayos T"));
            Assert.That(centralMessages["jawsTag"], Is.EqualTo("pinza hidráulica de rescate"));
            Assert.That(centralMessages["drillTag"], Is.EqualTo("taladro eléctrico"));
            Assert.That(centralMessages["fallbackReference"], Is.EqualTo("APC"));
            Assert.That(centralMessages["cableStack"], Is.EqualTo("cordón explosivo"));
            Assert.That(centralMessages["yakuzaShoes"], Is.EqualTo("zapatos de yakuza"));
            Assert.That(centralMessages["yellowBandanaHead"], Is.EqualTo("bandana amarilla"));
            Assert.That(centralMessages["yellowBandanaMask"], Is.EqualTo("bandana amarilla"));
            Assert.That(centralMessages["combatModeAction"], Is.EqualTo("[color=red]Modo de combate[/color]"));
            Assert.That(centralMessages["xenoResinWindow"], Is.EqualTo("ventana de resina"));
            Assert.That(centralMessages["shadyBox"], Is.EqualTo("caja sospechosa"));
            Assert.That(centralMessages["decalPlacer"], Is.EqualTo("Colocador de calcomanías"));
            Assert.That(centralMessages["tutorialExamine"], Does.Contain("Examina la esclusa de servicio"));
            Assert.That(centralMessages["roboticsBrain"], Is.EqualTo("[color=gray]Cerebro instalado:[/color] [color=green]Sí[/color]"));
            Assert.That(centralMessages["doubleStunbaton"], Is.EqualTo("double stunbaton"));
            Assert.That(centralMessages["usspCaptain"], Is.EqualTo("capitán de la USSP"));
            Assert.That(centralMessages["formattedUsspCaptain"], Is.EqualTo("Capitán de la USSP"));
            Assert.That(centralMessages["baseTutorials"], Is.EqualTo("Primeros pasos"));
            Assert.That(centralMessages["introductionTutorialName"], Is.EqualTo("Controles y mecánicas básicas"));
            Assert.That(centralMessages["introductionTutorialTooltip"], Is.EqualTo("Aprende los controles y las mecánicas básicas del juego."));
            Assert.That(centralMessages["guideException32"], Is.EqualTo("Excepción o precedente 3.2"));
            Assert.That(centralMessages["guideException34"], Is.EqualTo("Excepción o precedente 3.4"));
            Assert.That(centralMessages["guideException37"], Is.EqualTo("Excepción o precedente 3.7"));
            Assert.That(centralMessages["speciesDemon"], Is.EqualTo("Arcana"));
            Assert.That(centralMessages["speciesMilira"], Is.EqualTo("Milira"));
            Assert.That(centralMessages["lobbyAnimationScreen"], Is.EqualTo("Pantalla"));
            Assert.That(centralMessages["lobbyAnimationScp173"], Is.EqualTo("SCP-173"));
            Assert.That(centralMessages["lobbyAnimationMtf"], Is.EqualTo("Fuerza Operativa Móvil"));
            Assert.That(centralMessages["lobbyAnimationPc"], Is.EqualTo("Ordenador"));
            Assert.That(centralMessages["lobbyAnimationCamera"], Is.EqualTo("Cámara"));
            Assert.That(centralMessages["lobbyAnimationLogo"], Is.EqualTo("Logotipo"));
            Assert.That(centralMessages["lobbyAnimationRedGuy"], Is.EqualTo("Hombre de rojo"));
            Assert.That(centralMessages["lobbyAnimationScp049"], Is.EqualTo("SCP-049"));
            Assert.That(centralMessages["lobbyAnimationScp096"], Is.EqualTo("SCP-096"));
            Assert.That(centralMessages["lobbyAnimationFoundation"], Is.EqualTo("Fundación"));
            Assert.That(centralMessages["lobbyAnimationWorldMap"], Is.EqualTo("Mapa mundial"));
            Assert.That(centralMessages["lobbyAnimationDeepFacility"], Is.EqualTo("Instalación subterránea"));
            Assert.That(centralMessages["gameModeTie"], Is.EqualTo("¡Empate en la votación del modo de juego! Se ha elegido: Narrador"));
            Assert.That(centralMessages["gameModeWin"], Is.EqualTo("¡Narrador ha ganado la votación del modo de juego!"));
            Assert.That(centralMessages["mapTie"], Is.EqualTo("¡Empate en la votación del mapa! Se ha elegido: Complejo Compact"));
            Assert.That(centralMessages["mapWin"], Is.EqualTo("¡Complejo Compact ha ganado la votación del mapa!"));
            Assert.That(centralMessages["scp049CannotZombify"], Is.EqualTo("El organismo de Sujeto se resiste y no puede recibir el tratamiento."));
            Assert.That(centralMessages["areaReaction"], Does.Contain("reacción de humo o espuma"));
            Assert.That(centralMessages["areaReaction"], Does.Contain("2 segundos"));
            Assert.That(centralMessages["knockdownUpdate"], Does.Contain("derribo"));
            Assert.That(centralMessages["knockdownUpdate"], Does.Contain("2 segundos"));
            Assert.That(centralMessages["commandOfficesGrid"], Is.EqualTo("Oficinas del Mando"));
            Assert.That(centralMessages["transitHubGrid"], Is.EqualTo("Centro de distribución de la Fundación"));
            Assert.That(centralMessages["emergencyShuttleGrid"], Is.EqualTo("Transbordador blindado de evacuación"));
        });
    }
}
