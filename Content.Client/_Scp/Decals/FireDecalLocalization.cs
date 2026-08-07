using System.Text;
using Robust.Shared.Localization;

namespace Content.Client._Scp.Decals;

// Fire added start - локализация по соглашению для интерфейса размещения декалей.
public static class FireDecalLocalization
{
    public static string GetDecalName(
        ILocalizationManager localization,
        string decalId,
        string humanFallback)
    {
        return Resolve(localization, $"decal-name-{GetMessageSuffix(decalId)}", humanFallback);
    }

    public static string GetColorName(
        ILocalizationManager localization,
        string colorKey,
        string humanFallback)
    {
        return Resolve(localization, $"decal-color-{GetMessageSuffix(colorKey)}", humanFallback);
    }

    public static string GetCategoryName(
        ILocalizationManager localization,
        string tag,
        string humanFallback)
    {
        return Resolve(localization, $"decal-category-{GetMessageSuffix(tag)}", humanFallback);
    }

    public static string GetPaletteName(
        ILocalizationManager localization,
        string paletteId,
        string humanFallback)
    {
        return Resolve(localization, $"decal-palette-{GetMessageSuffix(paletteId)}", humanFallback);
    }

    public static string HumanizeIdentifier(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            return string.Empty;

        var text = new StringBuilder(identifier.Length + 8);
        for (var i = 0; i < identifier.Length; i++)
        {
            var current = identifier[i];
            if (current is '_' or '-')
            {
                if (text.Length > 0 && text[^1] != ' ')
                    text.Append(' ');

                continue;
            }

            if (i > 0 && identifier[i - 1] is not ('_' or '-'))
            {
                var previous = identifier[i - 1];
                var nextIsLower = i + 1 < identifier.Length && char.IsLower(identifier[i + 1]);
                var startsWord = char.IsUpper(current)
                    && (char.IsLower(previous) || char.IsDigit(previous) || char.IsUpper(previous) && nextIsLower);
                var changesCharacterKind = char.IsDigit(current) && char.IsLetter(previous)
                    || char.IsLetter(current) && char.IsDigit(previous);

                if ((startsWord || changesCharacterKind) && text.Length > 0 && text[^1] != ' ')
                    text.Append(' ');
            }

            text.Append(current);
        }

        var result = text.ToString().Trim();
        return result.Length == 0
            ? string.Empty
            : string.Concat(char.ToUpperInvariant(result[0]).ToString(), result.Substring(1));
    }

    private static string Resolve(
        ILocalizationManager localization,
        string messageId,
        string humanFallback)
    {
        return localization.TryGetString(messageId, out var localized)
            ? localized
            : humanFallback;
    }

    private static string GetMessageSuffix(string identifier)
    {
        var result = new StringBuilder(identifier.Length);
        foreach (var character in identifier)
        {
            if (char.IsAsciiLetterOrDigit(character) || character is '_' or '-')
            {
                result.Append(character);
                continue;
            }

            if (result.Length > 0 && result[^1] != '-')
                result.Append('-');
        }

        return result.ToString().Trim('-');
    }
}

public sealed record FireLocalizedDecalData(
    string TechnicalId,
    string DisplayName,
    IReadOnlyList<string> LocalizedCategories,
    IReadOnlySet<string>? TechnicalCategories = null)
{
    public bool MatchesCategory(string? category)
    {
        return category is null || TechnicalCategories?.Contains(category) == true;
    }

    public bool MatchesSearch(string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return true;

        var query = filter.Trim();
        if (DisplayName.Contains(query, StringComparison.CurrentCultureIgnoreCase))
            return true;

        foreach (var category in LocalizedCategories)
        {
            if (category.Contains(query, StringComparison.CurrentCultureIgnoreCase))
                return true;
        }

        return TechnicalId.Contains(query, StringComparison.OrdinalIgnoreCase);
    }
}
// Fire added end
