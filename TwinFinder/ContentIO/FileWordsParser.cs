namespace TwinFinder.ContentIO;

/** Converts file content to match the specification required */
public class FileWordsParser : IWordsParser {
    /** Parses files, and puts the words from them to usable format */
    public String[] parse(string filename, bool normalizeWords) {
        List<String> words = new List<string>();
        StreamReader reader = File.OpenText(filename);
        String? line;
        while ((line = reader.ReadLine()) != null) {
            String[] wordsOnLine = line.Split()
                .Select(StringModifications.getAlphabeticalPart)
                .Select(word => word.ToLower())
                .Where(word => word != "")
                .ToArray();
            if (normalizeWords) wordsOnLine = wordsOnLine.Select(StringModifications.removeDiacritics).ToArray();
            words.AddRange(wordsOnLine);
        }
        
        return words.ToArray();
    }
    
}