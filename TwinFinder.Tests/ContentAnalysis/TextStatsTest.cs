using System;
using System.Collections.Generic;
using NUnit.Framework;
using TwinFinder.ContentAnalysis;

namespace TwinFinder.Tests.ContentAnalysis;

[TestFixture]
[TestOf(typeof(TextStats))]
public class TextStatsTest {

    [Test]
    public void getFrequencies() {
        String[] words = ["some", "word", "common", "word", "some", "less"];
        Dictionary<String, int> result = TextStats.getFrequencies(words);
        Dictionary<String, int> expected = new Dictionary<String, int> {
            { "some", 2 }, { "word", 2 }, { "common", 1 }, { "less", 1 }
        };
        Assert.That(result, Is.EqualTo(expected));
    }
}