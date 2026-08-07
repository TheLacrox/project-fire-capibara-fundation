using Content.Shared.Maps;

namespace Content.Server.Voting.Managers;

public sealed partial class VoteManager
{
    internal readonly record struct MapVoteOption(GameMapPrototype Map, bool IsSecret);

    internal enum MapVoteAnnouncement
    {
        Secret,
        Winner,
        Tie,
    }

    internal static string GetUniqueMapVoteLabel(
        string localizedName,
        string mapId,
        IReadOnlySet<string> usedLabels)
    {
        if (!usedLabels.Contains(localizedName))
            return localizedName;

        var candidate = $"{localizedName} ({mapId})";
        var suffix = 2;
        while (usedLabels.Contains(candidate))
        {
            candidate = $"{localizedName} ({mapId} {suffix})";
            suffix++;
        }

        return candidate;
    }

    internal static MapVoteAnnouncement GetMapVoteAnnouncement(bool isSecret, bool isTie)
    {
        if (isSecret)
            return MapVoteAnnouncement.Secret;

        return isTie
            ? MapVoteAnnouncement.Tie
            : MapVoteAnnouncement.Winner;
    }
}
