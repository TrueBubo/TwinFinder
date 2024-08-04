namespace TwinFinder.ContentAnalysis;

public class Closest<K> where K : notnull {
    static double cosineSimilarity(Dictionary<K, int> v1, Dictionary<K, int> v2) {
        HashSet<K> keys = new HashSet<K>(v1.Keys);
        keys.UnionWith(v2.Keys);
        long dotProduct = 0;
        long magV1 = 0;
        long magV2 = 0;
        int elem1;
        int elem2;

        foreach (K key in keys) {
            elem1 = v1.ContainsKey(key) ? v1[key] : 0;
            elem2 = v2.ContainsKey(key) ? v2[key] : 0;
            dotProduct += elem1 * elem2;
            magV1 += elem1 * elem1;
            magV2 += elem2 * elem2;
        }

        return dotProduct / double.Sqrt(magV1 * magV2);
    }
}