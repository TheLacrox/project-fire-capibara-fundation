using System.Globalization;
using Content.Server._Sunrise.TTS;
using NUnit.Framework;

namespace Content.Tests.Server._Sunrise.TTS;

[TestFixture]
[TestOf(typeof(TTSSystem))]
public sealed class TtsSanitizerTest
{
    [Test]
    public void SpanishPreservesUnicodeLettersAndPunctuation()
    {
        var system = new TTSSystem();

        var result = system.SanitizeForCulture(
            "¿Quién pidió piñata número 3? 🚀",
            CultureInfo.GetCultureInfo("es-ES"));

        Assert.That(result, Is.EqualTo("¿Quién pidió piñata número 3?"));
    }

    [Test]
    public void EnglishIsNotTransliteratedToCyrillic()
    {
        var system = new TTSSystem();

        var result = system.SanitizeForCulture(
            "SCP-106 is here!",
            CultureInfo.GetCultureInfo("en-US"));

        Assert.That(result, Is.EqualTo("SCP-106 is here!"));
    }

    [Test]
    public void RussianRetainsLegacyPronunciationExpansion()
    {
        var system = new TTSSystem();

        var result = system.SanitizeForCulture(
            "SCP-106",
            CultureInfo.GetCultureInfo("ru-RU"));

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("Эс Си Пи"));
            Assert.That(result, Does.Contain("сто шесть"));
        });
    }
}
