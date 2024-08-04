using System.Text.Json;
using System.Text.Json.Serialization;

namespace TwinFinder.ContentAnalysis;

public class Synonyms {
    private class Word {
        public string word { get; set; }
        public List<string> synonyms { get; set; }
    }

    private static readonly Dictionary<String, String> SynonymsFile = new Dictionary<String, String>() {
        { "en", Path.Combine("SynonymsFiles", "synonyms-en.jsonl") }
    };

    private Dictionary<String, List<String>> _synonyms = new Dictionary<string, List<string>>();

    public Synonyms(String lang, int synonymCount) {
        if (!SynonymsFile.ContainsKey(lang)) {
            Console.Error.WriteLine($"Language file for {lang} was not found, available languages are:");
            foreach (String availableLang in SynonymsFile.Keys) Console.Error.WriteLine($"\t{availableLang}");
            Environment.Exit(1);
        }

        StreamReader reader = new StreamReader(SynonymsFile[lang]);
        String? line;
        while ((line = reader.ReadLine()) != null) {
            Word word = JsonSerializer.Deserialize<Word>(line) ?? new Word();
            _synonyms[word.word] = word.synonyms.Take(synonymCount).ToList();
        }
    }

    public Synonyms() {
    }

    public List<String> get(String word) {
        return _synonyms.ContainsKey(word) ? _synonyms[word] : new List<String>();
    }
}