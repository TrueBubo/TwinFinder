using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NUnit.Framework;
using TwinFinder.Configuration;
using TwinFinder.ContentAnalysis;

namespace TwinFinder.Tests.ContentAnalysis;

[TestFixture]
public class ClosestTest {
    private ConcurrentDictionary<String, Dictionary<String, int>> _vectors;
    private Closest<String> _closest;

    [SetUp]
    public void setUp() {
        _vectors = new ConcurrentDictionary<String, Dictionary<String, int>>();
        _vectors["file1"] = new Dictionary<String, Int32>();
        _vectors["file1"]["word"] = 3;
        _vectors["file1"]["less"] = 2;
        _vectors["file1"]["hello"] = 1;
        
        _vectors["file2"] = new Dictionary<String, Int32>();
        _vectors["file2"]["word"] = 3;
        _vectors["file2"]["less"] = 2;
        _vectors["file2"]["hello"] = 1;
                
         _vectors["file3"] = new Dictionary<String, Int32>();
         _vectors["file3"]["dog"] = 3;
         _vectors["file3"]["cat"] = 2;
         _vectors["file3"]["horse"] = 1;
            
        _closest = new Closest<String>(_vectors, new Options());
    }

    [Test]
    public void cosineSimilarity() {
        double result12 = Closest<String>.cosineSimilarity(_vectors["file1"], _vectors["file2"]);
        double result13 = Closest<String>.cosineSimilarity(_vectors["file1"], _vectors["file3"]);
        int similarity12Expected = 1;
        int similarity13Expected = 0;
        
        Assert.That(result12, Is.EqualTo(similarity12Expected));
        Assert.That(result13, Is.EqualTo(similarity13Expected));
    }
}