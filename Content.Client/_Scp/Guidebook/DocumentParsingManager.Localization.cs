using Content.Shared._Scp.Guidebook;
using Content.Shared.Guidebook;
using Robust.Shared.Localization;
using Robust.Shared.Utility;

namespace Content.Client.Guidebook;

public sealed partial class DocumentParsingManager
{
    [Dependency] private readonly ILocalizationManager _localization = default!;

    internal ResPath GetDocumentPath(GuideEntry entry)
    {
        return GuidebookLocalization.GetDocumentPath(
            entry.Text,
            _localization.DefaultCulture,
            _resourceManager.ContentFileExists);
    }
}
