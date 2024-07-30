using System.Collections.Concurrent;
using TwinFinder.ContentIO;

namespace TwinFinder.ContentAnalysis;

public class ProcessContent {
    ConcurrentDictionary<String, Dictionary<String, int>> _frequencies = new ConcurrentDictionary<String, Dictionary<string, int>>();
	ConcurrentBag<String> _uniqueWords = new ConcurrentBag<String>();
	
	
	public void processContent(String filename, IWordsParser parser, String mode, bool normalizeWords) {
		String[] words = parser.parse(filename, normalizeWords);
		
		switch (mode) {
			case "closest": {
				Dictionary<String, int> frequenciesFile = TextAnalyzer.getFrequencies(words);
				_frequencies[filename] = frequenciesFile;
				foreach (String word in frequenciesFile.Keys) {
					_uniqueWords.Add(word);
				}
				break; 
			}
			default: {
				throw new ArgumentException($"Mode {mode} does not exist");
			}
	}
	}
}