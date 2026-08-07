using System.Globalization;
using Robust.Shared.Utility;

namespace Content.Shared._Scp.Guidebook;

/// <summary>
/// Resolves culture-specific guidebook documents while preserving the base resource as fallback.
/// </summary>
public static class GuidebookLocalization
{
    private const string ServerInfoRoot = "/ServerInfo";
    private const string ServerInfoPrefix = ServerInfoRoot + "/";

    /// <summary>
    /// Returns a culture-specific mirror of a ServerInfo document when that resource exists.
    /// </summary>
    public static ResPath GetDocumentPath(
        ResPath source,
        CultureInfo? culture,
        Func<ResPath, bool> fileExists)
    {
        if (culture is null ||
            string.IsNullOrEmpty(culture.Name) ||
            !source.CanonPath.StartsWith(ServerInfoPrefix, StringComparison.Ordinal))
        {
            return source;
        }

        var localized = new ResPath($"{ServerInfoRoot}/{culture.Name}{source.CanonPath[ServerInfoRoot.Length..]}");
        return fileExists(localized) ? localized : source;
    }
}
