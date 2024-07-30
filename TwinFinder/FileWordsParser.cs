namespace TwinFinder;

public class FileWordsParser : IWordsParser {
    public List<string> parse(string filename, bool normalizeWords) {
        List<String> words = new List<string>();
        StreamReader reader = File.OpenText(filename);
        String? line;
        while ((line = reader.ReadLine()) != null) {
            String[] wordsOnLine = line.Split().Select(StringModifications.getAlphabeticalPart).Where(word => word != "").ToArray();
            if (normalizeWords) wordsOnLine = wordsOnLine.Select(StringModifications.removeDiacritics).ToArray();
            words.AddRange(wordsOnLine);
        }
        
        return words;
    }
    
}