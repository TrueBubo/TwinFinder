using System.Collections.Concurrent;

namespace TwinFinder;

public class ProcessFiles {
    ConcurrentDictionary<String, Dictionary<String, int>> frequencies = new ConcurrentDictionary<String, Dictionary<string, int>>();
	ConcurrentBag<String> uniqueWords = new ConcurrentBag<String>();
	
	
	public void processFile(String filename, String mode, bool normalizeWords) {
		FileWordsParser wordsReader = new FileWordsParser();
		List<String> words = wordsReader.parse(filename, normalizeWords);
		
		switch (mode) {
			case "closest": {
				Dictionary<String, int> frequenciesFile = TextAnalyzer.getFrequencies(words);
				frequencies[filename] = frequenciesFile;
				foreach (String word in frequenciesFile.Keys) {
					uniqueWords.Add(word);
				}
				break; 
			}
			default: {
				throw new ArgumentException($"Mode {mode} does not exist");
			}
	}
	}
}