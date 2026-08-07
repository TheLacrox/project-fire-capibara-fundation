using System.Collections.Generic;
using System.Globalization;
using Content.Client.Guidebook;
using Content.Client.Guidebook.Richtext;
using Content.Shared._Scp.Guidebook;
using Robust.Shared.ContentPack;
using Robust.Shared.Log;
using Robust.Shared.Prototypes;
using System.Linq;
using Content.Shared.Guidebook;
using Robust.Shared.Localization;
using Robust.UnitTesting;

namespace Content.IntegrationTests.Tests.Guidebook;

[TestFixture]
[TestOf(typeof(GuidebookSystem))]
[TestOf(typeof(GuideEntryPrototype))]
[TestOf(typeof(DocumentParsingManager))]
public sealed class GuideEntryPrototypeTests
{
    [Test]
    public async Task ValidatePrototypeContents()
    {
        await using var pair = await PoolManager.GetServerClient(new PoolSettings { Connected = true });
        var client = pair.Client;
        await client.WaitIdleAsync();
        // Sunrise-start: Данный тест невозможно нормально решить,
        // Так как у нас банально переполнен гайдбук реагентами. Оффы должны пофиксить когда-нибудь.
        await client.WaitPost(() => client.CfgMan.SetCVar(RTCVars.FailureLogLevel, LogLevel.Error));
        // Sunrise-end
        var protoMan = client.ResolveDependency<IPrototypeManager>();
        var resMan = client.ResolveDependency<IResourceManager>();
        var parser = client.ResolveDependency<DocumentParsingManager>();
        var localization = client.ResolveDependency<ILocalizationManager>();
        var prototypes = protoMan.EnumeratePrototypes<GuideEntryPrototype>().ToList();
        var localizedDocumentCount = 0; // Fire added - проверяем культурный путь, выбранный рабочим менеджером
        var unlocalizedDocuments = new List<string>(); // Fire added - ни одна видимая статья не должна уходить в fallback
        var documentsWithCyrillic = new List<string>(); // Fire added - русский текст не должен попадать в es-ES
        var englishDocumentsWithCyrillic = new List<string>(); // Fire added - fallback en-US также не должен зависеть от русского текста
        var missingLocalizedTitles = new List<string>(); // Fire added - дерево статей не должно показывать сырые LocId
        var invalidLocalizedTitles = new List<string>(); // Fire added - заголовки должны быть испанскими и читаемыми

        foreach (var proto in prototypes)
        {
            await client.WaitAssertion(() =>
            {
                using var reader = resMan.ContentFileReadText(proto.Text);
                var text = reader.ReadToEnd();
                Assert.That(parser.TryAddMarkup(new Document(), text), $"Failed to parse guidebook: {proto.Id}");
                // Fire added start - также проверяем документ, выбранный для активной культуры
                var selectedPath = parser.GetDocumentPath(proto);
                if (selectedPath != proto.Text)
                    localizedDocumentCount++;
                else
                    unlocalizedDocuments.Add(proto.Id);

                Assert.That(resMan.ContentFileExists(selectedPath), $"Localized guidebook does not exist: {selectedPath}");

                using var localizedReader = resMan.ContentFileReadText(selectedPath);
                var localizedText = localizedReader.ReadToEnd();
                Assert.That(
                    parser.TryAddMarkup(new Document(), localizedText),
                    $"Failed to parse localized guidebook: {proto.Id}");

                if (localizedText.Any(character => character is >= '\u0400' and <= '\u052F'))
                    documentsWithCyrillic.Add(proto.Id);

                var englishPath = GuidebookLocalization.GetDocumentPath(
                    proto.Text,
                    CultureInfo.GetCultureInfo("en-US"),
                    resMan.ContentFileExists);
                var englishText = text;
                if (englishPath != proto.Text)
                {
                    using var englishReader = resMan.ContentFileReadText(englishPath);
                    englishText = englishReader.ReadToEnd();
                    Assert.That(
                        parser.TryAddMarkup(new Document(), englishText),
                        $"Failed to parse English guidebook: {proto.Id}");
                }

                if (englishText.Any(character => character is >= '\u0400' and <= '\u052F'))
                    englishDocumentsWithCyrillic.Add(proto.Id);

                if (!localization.TryGetString(proto.Name, out var localizedTitle))
                    missingLocalizedTitles.Add(proto.Name);
                else if (localizedTitle == proto.Name ||
                         localizedTitle.Any(character => character is >= '\u0400' and <= '\u052F'))
                    invalidLocalizedTitles.Add($"{proto.Name}: {localizedTitle}");
                // Fire added end
            });

            // Avoid styleguide update limit
            await client.WaitRunTicks(1);
        }

        Assert.Multiple(() =>
        {
            Assert.That(unlocalizedDocuments, Is.Empty,
                $"Guidebook entries without an es-ES document: {string.Join(", ", unlocalizedDocuments)}");
            Assert.That(documentsWithCyrillic, Is.Empty,
                $"Spanish guidebook documents containing Cyrillic: {string.Join(", ", documentsWithCyrillic)}");
            Assert.That(englishDocumentsWithCyrillic, Is.Empty,
                $"English guidebook documents containing Cyrillic: {string.Join(", ", englishDocumentsWithCyrillic)}");
            Assert.That(missingLocalizedTitles, Is.Empty,
                $"Guidebook titles without localization: {string.Join(", ", missingLocalizedTitles)}");
            Assert.That(invalidLocalizedTitles, Is.Empty,
                $"Guidebook titles with raw LocIds or Cyrillic: {string.Join(", ", invalidLocalizedTitles)}");
            Assert.That(localizedDocumentCount, Is.EqualTo(prototypes.Count));
        }); // Fire added - каждая статья имеет испанское зеркало без русского текста

        await pair.CleanReturnAsync();
    }
}
