namespace TwinFinder;

public class FileWordsParser : IWordsParser {
    public List<string> parse(string filename) {
        List<String> words = new List<string>();
        StreamReader reader = File.OpenText(filename);
        String? line;
        while ((line = reader.ReadLine()) != null) {
            words.AddRange(line.Split().Select(getAlphabeticalPart).Where(word => word != "").ToArray());
        }
        return words;
    }

    private String getAlphabeticalPart(String word) {
        return new String(word.Where(char.IsLetter).ToArray());
    }
}