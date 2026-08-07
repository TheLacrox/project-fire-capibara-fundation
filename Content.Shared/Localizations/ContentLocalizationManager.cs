using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Robust.Shared.Utility;

namespace Content.Shared.Localizations
{
    public sealed class ContentLocalizationManager
    {
        [Dependency] private readonly ILocalizationManager _loc = default!;

        // If you want to change your codebase's language, do it here.
        // Fire edit start - испанская локализация Capibara Foundation
        private const string Culture = "es-ES";
        private const string FallbackCulture = "en-US";
        // Fire edit end

        /// <summary>
        /// Custom format strings used for parsing and displaying minutes:seconds timespans.
        /// </summary>
        public static readonly string[] TimeSpanMinutesFormats = new[]
        {
            @"m\:ss",
            @"mm\:ss",
            @"%m",
            @"mm"
        };

        public void Initialize()
        {
            var culture = new CultureInfo(Culture);
            var fallbackCulture = new CultureInfo(FallbackCulture);

            _loc.LoadCulture(culture);
            _loc.LoadCulture(fallbackCulture);
            _loc.SetFallbackCluture(fallbackCulture);
            // Fire edit start - общие функции нужны испанским строкам и английскому fallback
            foreach (var functionCulture in new[] { culture, fallbackCulture })
            {
                _loc.AddFunction(functionCulture, "PRESSURE", FormatPressure);
                _loc.AddFunction(functionCulture, "POWERWATTS", FormatPowerWatts);
                _loc.AddFunction(functionCulture, "POWERJOULES", FormatPowerJoules);
                // NOTE: ENERGYWATTHOURS() still takes a value in joules, but formats as watt-hours.
                _loc.AddFunction(functionCulture, "ENERGYWATTHOURS", FormatEnergyWattHours);
                _loc.AddFunction(functionCulture, "UNITS", FormatUnits);
                _loc.AddFunction(functionCulture, "TOSTRING", args => FormatToString(culture, args));
                _loc.AddFunction(functionCulture, "LOC", FormatLoc);
                _loc.AddFunction(functionCulture, "NATURALFIXED", FormatNaturalFixed);
                _loc.AddFunction(functionCulture, "NATURALPERCENT", FormatNaturalPercent);
                _loc.AddFunction(functionCulture, "PLAYTIME", FormatPlaytime);
            }
            // Fire added start - испанская форма «a» с артиклем
            _loc.AddFunction(culture, "AT-THE", FormatAtThe);
            // Fire added end
            // Fire added start - испанское множественное число для динамических единиц
            _loc.AddFunction(culture, "MAKEPLURAL", FormatMakePluralEs);
            // Fire added end
            // Fire edit end
            // Fire edit - испанские строки используют селекторы множественного числа вместо английского MANY().


            /*
             * The following language functions are specific to the english localization. When working on your own
             * localization you should NOT modify these, instead add new functions specific to your language/culture.
             * This ensures the english translations continue to work as expected when fallbacks are needed.
             */
            var cultureEn = new CultureInfo("en-US");

            _loc.AddFunction(cultureEn, "MAKEPLURAL", FormatMakePlural);
            _loc.AddFunction(cultureEn, "MANY", FormatMany);
        }

        private ILocValue FormatMany(LocArgs args)
        {
            var count = ((LocValueNumber) args.Args[1]).Value;

            if (Math.Abs(count - 1) < 0.0001f)
            {
                return (LocValueString) args.Args[0];
            }
            else
            {
                return (LocValueString) FormatMakePlural(args);
            }
        }

        // Fire added start - испанская форма «a» с артиклем
        private ILocValue FormatAtThe(LocArgs args)
        {
            return new LocValueString(_loc.GetString("zzzz-at-the", ("ent", args.Args[0])));
        }
        // Fire added end

        private ILocValue FormatNaturalPercent(LocArgs args)
        {
            var number = ((LocValueNumber) args.Args[0]).Value * 100;
            var maxDecimals = (int)Math.Floor(((LocValueNumber) args.Args[1]).Value);
            var formatter = (NumberFormatInfo)NumberFormatInfo.GetInstance(CultureInfo.GetCultureInfo(Culture)).Clone();
            formatter.NumberDecimalDigits = maxDecimals;
            return new LocValueString(string.Format(formatter, "{0:N}", number).TrimEnd('0').TrimEnd(char.Parse(formatter.NumberDecimalSeparator)) + "%");
        }

        private ILocValue FormatNaturalFixed(LocArgs args)
        {
            var number = ((LocValueNumber) args.Args[0]).Value;
            var maxDecimals = (int)Math.Floor(((LocValueNumber) args.Args[1]).Value);
            var formatter = (NumberFormatInfo)NumberFormatInfo.GetInstance(CultureInfo.GetCultureInfo(Culture)).Clone();
            formatter.NumberDecimalDigits = maxDecimals;
            return new LocValueString(string.Format(formatter, "{0:N}", number).TrimEnd('0').TrimEnd(char.Parse(formatter.NumberDecimalSeparator)));
        }

        private static readonly Regex PluralEsRule = new("^.*(s|sh|ch|x|z)$");
        // Fire added start - правила испанского множественного числа
        private static readonly Regex SpanishPluralWithSRule = new(@"[aeiouáéó]$", RegexOptions.IgnoreCase);
        private static readonly Regex SpanishPluralSOrXRule = new(@"[sx]$", RegexOptions.IgnoreCase);
        private static readonly Regex SpanishVowelGroupRule = new(@"[aeiouáéíóúü]+", RegexOptions.IgnoreCase);
        private static readonly Regex SpanishFinalStressRule = new(@"[áéíóú][^aeiouáéíóúü]*[nsx]$", RegexOptions.IgnoreCase);
        private static readonly HashSet<string> SpanishPhrasePrepositions = new()
        {
            "a", "ante", "bajo", "con", "contra", "de", "del", "desde", "durante", "en", "entre", "hacia",
            "hasta", "mediante", "para", "por", "según", "sin", "sobre", "tras",
        };
        // Fire added end
        // Fire added start - эвфонические союзы испанского языка
        private static readonly Regex EConjunctionEsRule = new(@"^(?:(?:i|hi)(?![aeouáéóú])|í|hí)", RegexOptions.IgnoreCase);
        private static readonly Regex UConjunctionEsRule = new(@"^(?:o|ho)", RegexOptions.IgnoreCase);
        // Fire added end

        private ILocValue FormatMakePlural(LocArgs args)
        {
            var text = ((LocValueString) args.Args[0]).Value;
            var split = text.Split(" ", 1);
            var firstWord = split[0];
            if (PluralEsRule.IsMatch(firstWord))
            {
                if (split.Length == 1)
                    return new LocValueString($"{firstWord}es");
                else
                    return new LocValueString($"{firstWord}es {split[1]}");
            }
            else
            {
                if (split.Length == 1)
                    return new LocValueString($"{firstWord}s");
                else
                    return new LocValueString($"{firstWord}s {split[1]}");
            }
        }

        // Fire added start - испанское множественное число
        private static ILocValue FormatMakePluralEs(LocArgs args)
        {
            var text = ((LocValueString) args.Args[0]).Value;
            return new LocValueString(FormatPluralEs(text));
        }

        /// <summary>
        /// Pluralizes the agreeing words of a simple Spanish noun phrase up to its first preposition.
        /// </summary>
        public static string FormatPluralEs(string text)
        {
            var words = text.Split(' ');
            var pluralize = true;

            for (var index = 0; index < words.Length; index++)
            {
                var word = words[index];
                if (word.Length == 0)
                    continue;

                if (index > 0 && SpanishPhrasePrepositions.Contains(word.ToLowerInvariant()))
                    pluralize = false;

                if (pluralize)
                    words[index] = FormatPluralWordEs(word);
            }

            return string.Join(' ', words);
        }

        private static string FormatPluralWordEs(string word)
        {

            string plural;
            if (word.EndsWith('z'))
                plural = $"{word[..^1]}ces";
            else if (word.EndsWith('Z'))
                plural = $"{word[..^1]}CES";
            else if (SpanishPluralSOrXRule.IsMatch(word))
            {
                var vowelGroups = SpanishVowelGroupRule.Matches(word).Count;
                var pluralStem = SpanishFinalStressRule.IsMatch(word)
                    ? RemoveAcuteAccent(word)
                    : word;
                plural = vowelGroups <= 1 || SpanishFinalStressRule.IsMatch(word)
                    ? $"{pluralStem}es"
                    : word;
            }
            else if (SpanishPluralWithSRule.IsMatch(word))
                plural = $"{word}s";
            else
            {
                var vowelGroups = SpanishVowelGroupRule.Matches(word);
                var pluralStem = word.EndsWith("n", StringComparison.OrdinalIgnoreCase) &&
                                 !SpanishFinalStressRule.IsMatch(word) &&
                                 vowelGroups.Count > 1
                    ? AddAcuteAccent(word, vowelGroups[^2])
                    : SpanishFinalStressRule.IsMatch(word)
                        ? RemoveAcuteAccent(word)
                        : word;
                plural = $"{pluralStem}es";
            }

            return plural;
        }

        private static string AddAcuteAccent(string text, Match vowelGroup)
        {
            var index = vowelGroup.Index;
            for (var offset = 0; offset < vowelGroup.Length; offset++)
            {
                var candidate = vowelGroup.Value[offset];
                if (candidate is 'a' or 'e' or 'o' or 'A' or 'E' or 'O')
                {
                    index += offset;
                    break;
                }

                if (offset == vowelGroup.Length - 1)
                    index += offset;
            }

            var accented = text[index] switch
            {
                'a' => 'á',
                'e' => 'é',
                'i' => 'í',
                'o' => 'ó',
                'u' => 'ú',
                'A' => 'Á',
                'E' => 'É',
                'I' => 'Í',
                'O' => 'Ó',
                'U' => 'Ú',
                _ => text[index],
            };

            return string.Concat(text.Substring(0, index), accented.ToString(), text.Substring(index + 1));
        }

        private static string RemoveAcuteAccent(string text)
        {
            return text
                .Replace('á', 'a')
                .Replace('é', 'e')
                .Replace('í', 'i')
                .Replace('ó', 'o')
                .Replace('ú', 'u')
                .Replace('Á', 'A')
                .Replace('É', 'E')
                .Replace('Í', 'I')
                .Replace('Ó', 'O')
                .Replace('Ú', 'U');
        }
        // Fire added end

        // Fire added start - испанские названия должностей используют регистр предложений
        /// <summary>
        /// Formats a localized title without capitalizing Spanish articles and prepositions.
        /// </summary>
        public static string FormatTitleCase(string text)
        {
            return FormatTitleCase(text, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Formats a localized title using the casing rules of the supplied culture.
        /// </summary>
        /// <param name="text">The title to format.</param>
        /// <param name="culture">The culture that owns the title.</param>
        /// <returns>The title with culture-appropriate casing.</returns>
        public static string FormatTitleCase(string text, CultureInfo culture)
        {
            if (!culture.Name.Equals(Culture, StringComparison.OrdinalIgnoreCase))
                return culture.TextInfo.ToTitleCase(text);

            if (string.IsNullOrEmpty(text))
                return text;

            return string.Concat(char.ToUpper(text[0], culture).ToString(), text.Substring(1));
        }
        // Fire added end

        // TODO: allow fluent to take in lists of strings so this can be a format function like it should be.
        /// <summary>
        /// Formats a list as per Spanish grammar rules.
        /// </summary>
        public static string FormatList(List<string> list)
        {
            // Fire edit start - испанский союз меняется перед звуком «и»
            var conjunction = list.Count > 1 && EConjunctionEsRule.IsMatch(list[^1].TrimStart()) ? "e" : "y";

            return list.Count switch
            {
                <= 0 => string.Empty,
                1 => list[0],
                2 => $"{list[0]} {conjunction} {list[1]}",
                _ => $"{string.Join(", ", list.GetRange(0, list.Count - 1))} {conjunction} {list[^1]}"
                // Fire edit end
            };
        }

        /// <summary>
        /// Formats a list as per Spanish grammar rules, but uses or instead of y.
        /// </summary>
        public static string FormatListToOr(List<string> list)
        {
            // Fire edit start - испанский союз меняется перед звуком «о»
            var conjunction = list.Count > 1 && UConjunctionEsRule.IsMatch(list[^1].TrimStart()) ? "u" : "o";

            return list.Count switch
            {
                <= 0 => string.Empty,
                1 => list[0],
                2 => $"{list[0]} {conjunction} {list[1]}",
                _ => $"{string.Join(", ", list.GetRange(0, list.Count - 1))} {conjunction} {list[^1]}"
                // Fire edit end
            };
        }

        /// <summary>
        /// Formats a direction struct as a human-readable string.
        /// </summary>
        public static string FormatDirection(Direction dir)
        {
            return Loc.GetString($"zzzz-fmt-direction-{dir.ToString()}");
        }

        /// <summary>
        /// Formats playtime as hours and minutes.
        /// </summary>
        public static string FormatPlaytime(TimeSpan time)
        {
            time = TimeSpan.FromMinutes(Math.Ceiling(time.TotalMinutes));
            var hours = (int)time.TotalHours;
            var minutes = time.Minutes;
            return Loc.GetString($"zzzz-fmt-playtime", ("hours", hours), ("minutes", minutes));
        }

        private static ILocValue FormatLoc(LocArgs args)
        {
            var id = ((LocValueString) args.Args[0]).Value;

            return new LocValueString(Loc.GetString(id, args.Options.Select(x => (x.Key, x.Value.Value!)).ToArray()));
        }

        private static ILocValue FormatToString(CultureInfo culture, LocArgs args)
        {
            var arg = args.Args[0];
            var fmt = ((LocValueString) args.Args[1]).Value;

            var obj = arg.Value;
            if (obj is IFormattable formattable)
                return new LocValueString(formattable.ToString(fmt, culture));

            return new LocValueString(obj?.ToString() ?? "");
        }

        private static ILocValue FormatUnitsGeneric(
            LocArgs args,
            string mode,
            Func<double, double>? transformValue = null)
        {
            const int maxPlaces = 5; // Matches amount in _lib.ftl
            var pressure = ((LocValueNumber) args.Args[0]).Value;

            if (transformValue != null)
                pressure = transformValue(pressure);

            var places = 0;
            while (pressure > 1000 && places < maxPlaces)
            {
                pressure /= 1000;
                places += 1;
            }

            return new LocValueString(Loc.GetString(mode, ("divided", pressure), ("places", places)));
        }

        private static ILocValue FormatPressure(LocArgs args)
        {
            return FormatUnitsGeneric(args, "zzzz-fmt-pressure");
        }

        private static ILocValue FormatPowerWatts(LocArgs args)
        {
            return FormatUnitsGeneric(args, "zzzz-fmt-power-watts");
        }

        private static ILocValue FormatPowerJoules(LocArgs args)
        {
            return FormatUnitsGeneric(args, "zzzz-fmt-power-joules");
        }

        private static ILocValue FormatEnergyWattHours(LocArgs args)
        {
            const double joulesToWattHours = 1.0 / 3600;

            return FormatUnitsGeneric(args, "zzzz-fmt-energy-watt-hours", joules => joules * joulesToWattHours);
        }

        private static ILocValue FormatUnits(LocArgs args)
        {
            if (!Units.Types.TryGetValue(((LocValueString) args.Args[0]).Value, out var ut))
                throw new ArgumentException($"Unknown unit type {((LocValueString) args.Args[0]).Value}");

            var fmtstr = ((LocValueString) args.Args[1]).Value;

            double max = Double.NegativeInfinity;
            var iargs = new double[args.Args.Count - 1];
            for (var i = 2; i < args.Args.Count; i++)
            {
                var n = ((LocValueNumber) args.Args[i]).Value;
                if (n > max)
                    max = n;

                iargs[i - 2] = n;
            }

            if (!ut.TryGetUnit(max, out var mu))
                throw new ArgumentException("Unit out of range for type");

            var fargs = new object[iargs.Length];

            for (var i = 0; i < iargs.Length; i++)
                fargs[i] = iargs[i] * mu.Factor;

            fargs[^1] = Loc.GetString($"units-{mu.Unit.ToLower()}");

            // Before anyone complains about "{"+"${...}", at least it's better than MS's approach...
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/composite-formatting#escaping-braces
            //
            // Note that the closing brace isn't replaced so that format specifiers can be applied.
            var res = String.Format(
                fmtstr.Replace("{UNIT", "{" + $"{fargs.Length - 1}"),
                fargs
            );

            return new LocValueString(res);
        }

        private static ILocValue FormatPlaytime(LocArgs args)
        {
            var time = TimeSpan.Zero;
            if (args.Args is { Count: > 0 } && args.Args[0].Value is TimeSpan timeArg)
            {
                time = timeArg;
            }
            return new LocValueString(FormatPlaytime(time));
        }
    }
}
