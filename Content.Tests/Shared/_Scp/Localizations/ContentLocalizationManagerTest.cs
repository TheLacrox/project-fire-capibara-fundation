using System.Collections.Generic;
using System.Globalization;
using Content.Shared.Localizations;
using NUnit.Framework;

namespace Content.Tests.Shared._Scp.Localizations
{
    [TestFixture]
    public sealed class ContentLocalizationManagerTest
    {
        [TestCase("informe", "manual e informe")]
        [TestCase("índice", "manual e índice")]
        [TestCase("hígado", "manual e hígado")]
        [TestCase("hielo", "manual y hielo")]
        [TestCase("hiato", "manual y hiato")]
        public void FormatListUsesSpanishConjunction(string finalItem, string expected)
        {
            var result = ContentLocalizationManager.FormatList(new List<string> { "manual", finalItem });

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void FormatListToOrUsesUBeforeO()
        {
            var result = ContentLocalizationManager.FormatListToOr(new List<string> { "norte", "oeste" });

            Assert.That(result, Is.EqualTo("norte u oeste"));
        }

        [TestCase("lámina", "láminas")]
        [TestCase("lingote", "lingotes")]
        [TestCase("tabla", "tablas")]
        [TestCase("rollo", "rollos")]
        [TestCase("pieza", "piezas")]
        [TestCase("racimo", "racimos")]
        [TestCase("hebra", "hebras")]
        [TestCase("trozo", "trozos")]
        [TestCase("mota", "motas")]
        [TestCase("billete", "billetes")]
        [TestCase("saco", "sacos")]
        [TestCase("vale de expedición", "vales de expedición")]
        [TestCase("papel", "papeles")]
        [TestCase("luz", "luces")]
        [TestCase("mes", "meses")]
        [TestCase("gas", "gases")]
        [TestCase("inglés", "ingleses")]
        [TestCase("fax", "faxes")]
        [TestCase("compás", "compases")]
        [TestCase("autobús", "autobuses")]
        [TestCase("estación", "estaciones")]
        [TestCase("jamón", "jamones")]
        [TestCase("estación auxiliar", "estaciones auxiliares")]
        [TestCase("joven", "jóvenes")]
        [TestCase("crisis", "crisis")]
        [TestCase("lunes", "lunes")]
        [TestCase("tórax", "tórax")]
        public void FormatPluralEsUsesSpanishRules(string singular, string expected)
        {
            var result = ContentLocalizationManager.FormatPluralEs(singular);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("es-ES", "director del complejo", "Director del complejo")]
        [TestCase("es-ES", "Director del complejo", "Director del complejo")]
        [TestCase("es-ES", "Chief Medical Officer", "Chief Medical Officer")]
        [TestCase("es-ES", "NanoTrasen Representative", "NanoTrasen Representative")]
        [TestCase("en-US", "station engineer", "Station Engineer")]
        public void FormatTitleCaseRespectsLocalizedCasing(string cultureName, string text, string expected)
        {
            var culture = CultureInfo.GetCultureInfo(cultureName);

            var result = ContentLocalizationManager.FormatTitleCase(text, culture);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
