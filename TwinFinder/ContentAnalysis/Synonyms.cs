using System.Text.Json;
using TwinFinder.Configuration;

namespace TwinFinder.ContentAnalysis;

/** Loads synonyms of words
 * Synonyms will depend on set language
 */
public class Synonyms {
    /** Data class for storing words with their synonyms */
    private class Word {
        public string word { get; set; } = ""; public List<string> synonyms { get; set; } = new List<string>();
    }

    /** Files with synonyms for given languages */
    private static readonly Dictionary<Options.Language, String> SynonymsFile = new Dictionary<Options.Language, String>() {
        { Options.Language.English, "synonyms-en.jsonl" }
    };
    
    private Dictionary<String, List<String>> _synonyms = new Dictionary<String, List<String>>();

    public Synonyms(Options.Language lang, int synonymCount, String? shared) {
        if (!SynonymsFile.ContainsKey(lang)) {
            Console.Error.WriteLine($"Language file for {Options.langToCode[lang]} was not found, available languages are:");
            foreach (Options.Language availableLang in SynonymsFile.Keys) Console.Error.WriteLine($"\t{Options.langToCode[availableLang]}");
            Environment.Exit(1);
        }

		String synonymsDir = shared != null ? Path.Combine(shared, "SynonymsFiles") : "SynonymsFiles"; 
		String synonymsFile = Path.Combine(synonymsDir, SynonymsFile[lang]); 
		_synonyms = loadSynonyms(synonymsFile, synonymCount); 
	}

    public Synonyms() {
    }

    /** Loads synonyms from file
     * @param synonymFile File to get synonyms from
     * @param synonymCount Maximum number of synonyms for one word
     * @return Dictionary with words as keys and synonyms as values
     */
    private Dictionary<String, List<String>> loadSynonyms(String synonymFile, int synonymCount) {
        Dictionary<String, List<String>> result = new();
        StreamReader reader = new StreamReader(synonymFile);
        String? line;
        while ((line = reader.ReadLine()) != null) {
        if (line[0] == '/') continue; // Comment
            Word word = JsonSerializer.Deserialize<Word>(line) ?? new Word(); 
            result[word.word] = word.synonyms.Take(synonymCount).ToList();
        }
        return result;
    }
    
    public List<String> get(String word) {
        return _synonyms.ContainsKey(word) ? _synonyms[word] : new List<String>();
    }
}