namespace TwinFinder;

public class ProcessFiles {
    Dictionary<String, Dictionary<String, int>> frequencies = new Dictionary<String, Dictionary<string, int>>();
	HashSet<String> uniqueWords = new HashSet<string>();
	
	
	public void processFile(String filename, String mode, bool normalizeWords) {
		FileWordsParser wordsReader = new FileWordsParser();
		List<String> words = wordsReader.parse(filename, normalizeWords);
		foreach (var word in words) {
			Console.WriteLine(word);
			
		}
		
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