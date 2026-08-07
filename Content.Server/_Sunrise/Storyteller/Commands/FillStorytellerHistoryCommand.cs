using Content.Server.Administration;
using Content.Server._Sunrise.Storyteller.Systems;
using Content.Shared._Sunrise.Storyteller;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server._Sunrise.Storyteller.Commands;

[AdminCommand(AdminFlags.Debug)]
public sealed class FillStorytellerHistoryCommand : IConsoleCommand
{
    public string Command => "fill_storyteller_history";
    public string Description => Loc.GetString("storyteller-debug-history-command-description"); // Fire edit - Локализация команды.
    public string Help => Loc.GetString("storyteller-debug-history-command-help");

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var historySystem = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<StorytellerHistorySystem>();

        foreach (var type in Enum.GetValues<StorytellerHistoryType>())
        {
            var locKey = type switch
            {
                StorytellerHistoryType.HelpfulEvent => "storyteller-history-event-started",
                StorytellerHistoryType.NeutralEvent => "storyteller-history-event-started",
                StorytellerHistoryType.MinorCalmEvent => "storyteller-history-event-started",
                StorytellerHistoryType.MajorCalmEvent => "storyteller-history-event-started",
                StorytellerHistoryType.MinorAntagEvent => "storyteller-history-threat-started",
                StorytellerHistoryType.MajorAntagEvent => "storyteller-history-threat-started",
                StorytellerHistoryType.Death => "storyteller-history-crew-death-1",
                StorytellerHistoryType.AnomalyEngine => "storyteller-history-singularity-escaped",
                StorytellerHistoryType.Explosion => "storyteller-history-large-explosion",
                StorytellerHistoryType.Research => "storyteller-history-research-complete",
                StorytellerHistoryType.Arrival => "storyteller-history-arrival",
                StorytellerHistoryType.Departure => "storyteller-history-cryo-departure",
                StorytellerHistoryType.StationEvent => "storyteller-history-alert-level-changed",
                _ => "storyteller-history-event-started"
            };

            historySystem.LogHistoryEntry(type, locKey, 
                ("name", Loc.GetString("storyteller-debug-history-event")),
                ("job", Loc.GetString("storyteller-debug-history-job")),
                ("location", Loc.GetString("storyteller-debug-history-location")),
                ("cause", Loc.GetString("storyteller-debug-history-cause")),
                ("severity", Loc.GetString("storyteller-debug-history-severity")),
                ("discipline", Loc.GetString("storyteller-debug-history-discipline")),
                ("level", Loc.GetString("alert-level-red")),
                ("color", "#ff0000"));
        }

        historySystem.LogHistoryEntry(StorytellerHistoryType.StationEvent, "storyteller-history-alert-level-changed-with-prev", 
            ("level", Loc.GetString("alert-level-red")),
            ("color", "#ff0000"),
            ("prev", Loc.GetString("alert-level-blue")),
            ("prevColor", "#0000ff"),
            ("duration", 10));

        historySystem.LogHistoryEntry(StorytellerHistoryType.StationEvent, "storyteller-history-nuke-armed", 
            ("location", Loc.GetString("storyteller-debug-history-captain-quarters")));
            
        historySystem.LogHistoryEntry(StorytellerHistoryType.StationEvent, "storyteller-history-nuke-disarmed");

        shell.WriteLine(Loc.GetString("storyteller-debug-history-command-success"));
    }
}
