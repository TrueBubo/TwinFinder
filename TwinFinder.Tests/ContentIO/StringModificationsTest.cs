using System;
using NUnit.Framework;
using TwinFinder.ContentIO;

namespace TwinFinder.Tests.ContentIO;

[TestFixture]
[TestOf(typeof(StringModifications))]
public class StringModificationsTest {

    [Test]
    [TestCase("naïve", "naive")]
    [TestCase("déjà vu", "deja vu")]
    public void removeDiacritics(String withDiacritics, String expected) {
        String result = StringModifications.removeDiacritics(withDiacritics);
        Assert.That(result, Is.EqualTo(expected));
    }
}