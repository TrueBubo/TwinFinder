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

    public Synonyms(Options.Language lang, int synonymCount, String synonymsLoc) {
        if (!SynonymsFile.ContainsKey(lang)) {
            _synonyms = new Dictionary<String, List<String>>();
            return;
        }
        
        String synonymsFile = Path.Combine(synonymsLoc, SynonymsFile[lang]);
        _synonyms = loadSynonyms(synonymsFile, synonymCount);
        return;
        
        
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

    public static String langNotFoundMessage(String lang) {
         String text = $"Language file for {lang} was not found, available languages are:\n";
         foreach (Options.Language availableLang in SynonymsFile.Keys) text += $"\t{Options.langToCode[availableLang]}\n";
         text += "The program will process content without any context\n";
         return text;
    }
}