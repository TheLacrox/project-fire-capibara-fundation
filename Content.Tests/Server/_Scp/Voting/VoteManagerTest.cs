using System.Collections.Generic;
using Content.Server.Voting.Managers;
using NUnit.Framework;

namespace Content.Tests.Server._Scp.Voting;

[TestFixture]
public sealed class VoteManagerTest
{
    [Test]
    public void MapVoteLabelsDisambiguateLocalizedCollisions()
    {
        var usedLabels = new HashSet<string> { "Secreto", "Complejo" };

        var secretCollision = VoteManager.GetUniqueMapVoteLabel("Secreto", "SecretMap", usedLabels);
        usedLabels.Add(secretCollision);
        var translatedCollision = VoteManager.GetUniqueMapVoteLabel("Complejo", "SecondComplex", usedLabels);

        Assert.Multiple(() =>
        {
            Assert.That(secretCollision, Is.EqualTo("Secreto (SecretMap)"));
            Assert.That(translatedCollision, Is.EqualTo("Complejo (SecondComplex)"));
        });
    }

    [Test]
    public void SecretMapVoteOptionRemainsDistinctFromVisibleOption()
    {
        var secret = new VoteManager.MapVoteOption(null!, true);
        var visible = new VoteManager.MapVoteOption(null!, false);

        Assert.Multiple(() =>
        {
            Assert.That(secret.IsSecret, Is.True);
            Assert.That(secret, Is.Not.EqualTo(visible));
        });
    }

    [TestCase(true, false, "Secret")]
    [TestCase(true, true, "Secret")]
    [TestCase(false, false, "Winner")]
    [TestCase(false, true, "Tie")]
    public void MapVoteAnnouncementPreservesSecretSelection(bool isSecret, bool isTie, string expected)
    {
        Assert.That(VoteManager.GetMapVoteAnnouncement(isSecret, isTie).ToString(), Is.EqualTo(expected));
    }
}
