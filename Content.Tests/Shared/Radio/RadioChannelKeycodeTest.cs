#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Content.Shared.Chat;
using Content.Shared.Radio;
using NUnit.Framework;
using YamlDotNet.RepresentationModel;

namespace Content.Tests.Shared.Radio;

[TestFixture]
[TestOf(typeof(RadioChannelPrototype))]
public sealed class RadioChannelKeycodeTest
{
    private static readonly IReadOnlyDictionary<string, char> ExpectedStandardKeycodes =
        new Dictionary<string, char>
        {
            ["Common"] = ';',
            ["CentCom"] = 'y',
            ["Command"] = 'c',
            ["Engineering"] = 'e',
            ["Medical"] = 'm',
            ["Science"] = 'n',
            ["Security"] = 's',
            ["Service"] = 'v',
            ["Supply"] = 'u',
            ["Syndicate"] = 't',
            ["Binary"] = 'b',
            ["Freelance"] = 'f',
            ["Xenoborg"] = 'x',
            ["Mothership"] = 'o',
        };

    [Test]
    public void RadioChannelKeycodesAreReachableAndLatinCompatible()
    {
        var keycodes = LoadRadioChannelKeycodes();
        var assigned = keycodes
            .Where(entry => entry.Value != '\0')
            .ToList();
        var duplicateKeycodes = assigned
            .GroupBy(entry => char.ToLowerInvariant(entry.Value))
            .Where(group => group.Count() > 1)
            .Select(group => $"{group.Key}: {string.Join(", ", group.Select(entry => entry.Key))}")
            .ToList();
        var nonAsciiKeycodes = assigned
            .Where(entry => entry.Value > 0x7F)
            .Select(entry => $"{entry.Key}: {entry.Value}")
            .ToList();
        var uppercaseKeycodes = assigned
            .Where(entry => entry.Value != char.ToLowerInvariant(entry.Value))
            .Select(entry => $"{entry.Key}: {entry.Value}")
            .ToList();
        var parserLookup = new Dictionary<char, string>();

        foreach (var (id, keycode) in assigned)
            parserLookup.TryAdd(char.ToLowerInvariant(keycode), id);

        Assert.Multiple(() =>
        {
            Assert.That(SharedChatSystem.DefaultChannelKey, Is.EqualTo('h'));
            Assert.That(parserLookup.GetValueOrDefault('b'), Is.EqualTo("Binary"));
            Assert.That(parserLookup.GetValueOrDefault('s'), Is.EqualTo("Security"));
            Assert.That(duplicateKeycodes, Is.Empty);
            Assert.That(nonAsciiKeycodes, Is.Empty);
            Assert.That(uppercaseKeycodes, Is.Empty);
            Assert.That(assigned.All(entry => entry.Value != SharedChatSystem.DefaultChannelKey), Is.True);

            foreach (var (id, keycode) in ExpectedStandardKeycodes)
                Assert.That(keycodes.GetValueOrDefault(id), Is.EqualTo(keycode), id);
        });
    }

    private static Dictionary<string, char> LoadRadioChannelKeycodes()
    {
        var repositoryRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", ".."));
        var prototypeRoot = Path.Combine(repositoryRoot, "Resources", "Prototypes");
        var keycodes = new Dictionary<string, char>();

        foreach (var file in Directory.EnumerateFiles(prototypeRoot, "*.yml", SearchOption.AllDirectories))
        {
            var yaml = File.ReadAllText(file);
            if (!yaml.Contains("type: radioChannel", StringComparison.Ordinal))
                continue;

            var stream = new YamlStream();
            stream.Load(new StringReader(yaml));

            foreach (var document in stream.Documents)
            {
                if (document.RootNode is not YamlSequenceNode prototypes)
                    continue;

                foreach (var prototype in prototypes.OfType<YamlMappingNode>())
                {
                    if (!TryGetScalar(prototype, "type", out var type) || type != "radioChannel")
                        continue;

                    Assert.That(TryGetScalar(prototype, "id", out var id), Is.True, file);
                    var keycode = '\0';
                    if (TryGetScalar(prototype, "keycode", out var serializedKeycode))
                    {
                        Assert.That(serializedKeycode, Has.Length.EqualTo(1), id);
                        keycode = serializedKeycode[0];
                    }

                    Assert.That(keycodes.TryAdd(id, keycode), Is.True, $"Canal duplicado: {id}");
                }
            }
        }

        Assert.That(keycodes, Is.Not.Empty);
        return keycodes;
    }

    private static bool TryGetScalar(YamlMappingNode mapping, string key, out string value)
    {
        if (mapping.Children.TryGetValue(new YamlScalarNode(key), out var node) &&
            node is YamlScalarNode { Value: { } scalar })
        {
            value = scalar;
            return true;
        }

        value = string.Empty;
        return false;
    }
}
