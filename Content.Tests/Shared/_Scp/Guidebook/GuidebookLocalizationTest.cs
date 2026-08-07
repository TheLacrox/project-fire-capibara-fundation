using System.Globalization;
using Content.Shared._Scp.Guidebook;
using NUnit.Framework;
using Robust.Shared.Utility;

namespace Content.Tests.Shared._Scp.Guidebook;

[TestFixture]
public sealed class GuidebookLocalizationTest
{
    [Test]
    public void GetDocumentPathUsesCultureSpecificResourceWhenAvailable()
    {
        var source = new ResPath("/ServerInfo/_Scp/Guidebook/Research/Research.xml");
        var localized = new ResPath("/ServerInfo/es-ES/_Scp/Guidebook/Research/Research.xml");

        var result = GuidebookLocalization.GetDocumentPath(
            source,
            CultureInfo.GetCultureInfo("es-ES"),
            path => path == localized);

        Assert.That(result, Is.EqualTo(localized));
    }

    [Test]
    public void GetDocumentPathFallsBackToBaseResourceWhenTranslationIsMissing()
    {
        var source = new ResPath("/ServerInfo/_Scp/Guidebook/Research/Research.xml");

        var result = GuidebookLocalization.GetDocumentPath(
            source,
            CultureInfo.GetCultureInfo("es-ES"),
            _ => false);

        Assert.That(result, Is.EqualTo(source));
    }

    [Test]
    public void GetDocumentPathDoesNotRewriteResourcesOutsideServerInfo()
    {
        var source = new ResPath("/Textures/Interface/logo.png");

        var result = GuidebookLocalization.GetDocumentPath(
            source,
            CultureInfo.GetCultureInfo("es-ES"),
            _ => true);

        Assert.That(result, Is.EqualTo(source));
    }
}
