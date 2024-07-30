namespace TwinFinder.ContentAnalysis;

public static class TextAnalyzer {
    
    public static Dictionary<String, int> getFrequencies(List<String> words) {
        Dictionary<String, int> frequencies = new Dictionary<String, int>();
        foreach (String word in words) {
            if (frequencies.ContainsKey(word)) frequencies[word]++;
            else frequencies[word] = 1;
        }

        return frequencies;
    }
}