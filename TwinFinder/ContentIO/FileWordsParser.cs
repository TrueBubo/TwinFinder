namespace TwinFinder.ContentIO;

/** Converts file content to match the specification required */
public class FileWordsParser : IWordsParser {
    /** Parses files, and puts the words from them to usable format */
    public String[] parse(string filename, bool normalizeWords) {
        List<String> words = new List<string>();
        if (isBinary(filename)) return []; // Do not waste time looking at binary formats (images, videos etc.)
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

    // I want to thank @bytedev for providing this code, https://stackoverflow.com/a/64038750
    public bool isBinary(string filePath, int requiredConsecutiveNul = 1) {
        const int charsToCheck = 8000;
        const char nulChar = '\0';

        int nulCount = 0;

        using (var streamReader = new StreamReader(filePath)) {
            for (var i = 0; i < charsToCheck; i++) {
                if (streamReader.EndOfStream)
                    return false;

                if ((char)streamReader.Read() == nulChar) {
                    nulCount++;

                    if (nulCount >= requiredConsecutiveNul)
                        return true;
                }
                else {
                    nulCount = 0;
                }
            }
        }

        return false;
    }
}