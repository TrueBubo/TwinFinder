using System;
using System.Collections;
using NUnit.Framework;
using TwinFinder.Configuration;

namespace TwinFinder.Tests.Configuration;

[TestFixture]
[TestOf(typeof(ArgsParser))]
public class ArgsParserTest {
    private ArgsParser _argsParser;

    [SetUp]
    public void setUp() {
        _argsParser = new ArgsParser();
    }
    
    [Test]
    public void parseOptions() {
        String[] args = {"-n", "5", "-s", "2" };
        ArgsParser.Args expected = new ArgsParser.Args(new Hashtable { { "pairsToFind", "5" }, { "synonymCount", "2" } }, new String[]{});
        ArgsParser.Args result = _argsParser.parse(args);
        Assert.That(result.options, Is.EqualTo(expected.options));
    }
}