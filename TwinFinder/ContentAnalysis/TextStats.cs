namespace TwinFinder.ContentAnalysis;

/** Gets stats about a specific text */
public static class TextStats {
    /** Finds how frequent words are in the given list
     * @param words List of words as they appear in a text
     * @return Dictionary with words as keys and how often they appear in the text
     */
    public static Dictionary<String, int> getFrequencies(String[] words) {
        Dictionary<String, int> frequencies = new Dictionary<String, int>();
        foreach (String word in words) {
            if (frequencies.ContainsKey(word)) frequencies[word]++;
            else frequencies[word] = 1;
        }

        return frequencies;
    }
}