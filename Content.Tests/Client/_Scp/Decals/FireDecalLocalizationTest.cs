#nullable enable

using System.Collections.Generic;
using Content.Client._Scp.Decals;
using Moq;
using NUnit.Framework;
using Robust.Shared.Localization;

namespace Content.Tests.Client._Scp.Decals;

[TestFixture]
public sealed class FireDecalLocalizationTest
{
    [Test]
    public void DecalNameUsesConventionalLocalizedMessage()
    {
        var localization = new Mock<ILocalizationManager>();
        var localizedName = "Superposición de baldosa completa";
        localization
            .Setup(manager => manager.TryGetString("decal-name-FullTileOverlayGreyscale", out localizedName))
            .Returns(true);

        var result = FireDecalLocalization.GetDecalName(
            localization.Object,
            "FullTileOverlayGreyscale",
            "Superposición de baldosa");

        Assert.That(result, Is.EqualTo(localizedName));
        localization.Verify(
            manager => manager.TryGetString("decal-name-FullTileOverlayGreyscale", out localizedName),
            Times.Once);
    }

    [Test]
    public void MissingColorMessageUsesExplicitHumanFallback()
    {
        var localization = new Mock<ILocalizationManager>();

        var result = FireDecalLocalization.GetColorName(
            localization.Object,
            "dark-red",
            "Rojo oscuro");

        Assert.That(result, Is.EqualTo("Rojo oscuro"));
        Assert.That(result, Is.Not.EqualTo("decal-color-dark-red"));
    }

    [Test]
    public void CategoryUsesConventionalLocalizedMessage()
    {
        var localization = new Mock<ILocalizationManager>();
        var localizedCategory = "Estación";
        localization
            .Setup(manager => manager.TryGetString("decal-category-station", out localizedCategory))
            .Returns(true);

        var result = FireDecalLocalization.GetCategoryName(
            localization.Object,
            "station",
            "Estación espacial");

        Assert.That(result, Is.EqualTo(localizedCategory));
    }

    [Test]
    public void PaletteUsesPrototypeIdForConventionalLocalizedMessage()
    {
        var localization = new Mock<ILocalizationManager>();
        var localizedPalette = "Departamentos";
        localization
            .Setup(manager => manager.TryGetString("decal-palette-Departmental", out localizedPalette))
            .Returns(true);

        var result = FireDecalLocalization.GetPaletteName(
            localization.Object,
            "Departmental",
            "Departmental");

        Assert.That(result, Is.EqualTo(localizedPalette));
    }

    [Test]
    public void ColorUsesExactTechnicalKeyForConventionalLocalizedMessage()
    {
        var localization = new Mock<ILocalizationManager>();
        var localizedColor = "Tecnicolor azul";
        localization
            .Setup(manager => manager.TryGetString("decal-color-TechniBlue", out localizedColor))
            .Returns(true);

        var result = FireDecalLocalization.GetColorName(
            localization.Object,
            "TechniBlue",
            "Techni Blue");

        Assert.That(result, Is.EqualTo(localizedColor));
    }

    [Test]
    public void ColorWithSpacesUsesValidMessageIdWithoutChangingTechnicalFallback()
    {
        var localization = new Mock<ILocalizationManager>();
        var localizedColor = "Azul claro";
        localization
            .Setup(manager => manager.TryGetString("decal-color-light-blue", out localizedColor))
            .Returns(true);

        var result = FireDecalLocalization.GetColorName(
            localization.Object,
            "light blue",
            "Light blue");

        Assert.That(result, Is.EqualTo(localizedColor));
    }

    [TestCase("FullTileOverlayGreyscale", "Full Tile Overlay Greyscale")]
    [TestCase("BrickCornerOverlayNE", "Brick Corner Overlay NE")]
    [TestCase("dark_red", "Dark red")]
    [TestCase("security-blue", "Security blue")]
    public void IdentifierFallbackIsHumanReadable(string identifier, string expected)
    {
        Assert.That(FireDecalLocalization.HumanizeIdentifier(identifier), Is.EqualTo(expected));
    }

    [Test]
    public void LocalizedDecalDataPreservesTechnicalIdMetadata()
    {
        var data = new FireLocalizedDecalData(
            "TechnicalDecal42",
            "Señal localizada",
            new[] { "Estación" });

        Assert.That(data.TechnicalId, Is.EqualTo("TechnicalDecal42"));
        Assert.That(data.DisplayName, Is.EqualTo("Señal localizada"));
    }

    [TestCase("señal")]
    [TestCase("estación")]
    [TestCase("technicaldecal42")]
    public void SearchMatchesLocalizedNameCategoryAndSecondaryTechnicalId(string query)
    {
        var data = new FireLocalizedDecalData(
            "TechnicalDecal42",
            "Señal localizada",
            new[] { "Estación" });

        Assert.That(data.MatchesSearch(query), Is.True);
    }

    [TestCase(null, true)]
    [TestCase("station", true)]
    [TestCase("hazard", false)]
    public void CategoryFilterMatchesTechnicalTagWithoutExposingIt(string? category, bool expected)
    {
        var data = new FireLocalizedDecalData(
            "TechnicalDecal42",
            "Señal localizada",
            new[] { "Estación" },
            new HashSet<string> { "station" });

        Assert.That(data.MatchesCategory(category), Is.EqualTo(expected));
    }
}
