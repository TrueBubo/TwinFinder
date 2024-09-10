using System.Collections.Concurrent;
using TwinFinder.Configuration;

//! Handles statistical analysis of contents, including finding lexicographically similar contents
namespace TwinFinder.ContentAnalysis;

/** Finds most similar contents based on how close they are lexicographically.
 * @param TK Datatype of words
 */
public class Closest<TK> where TK : notnull {
    /** Frequencies of words in content.
     * (Location, (Word, Frequency))
     */
    private readonly ConcurrentDictionary<String, Dictionary<TK, int>> _vectors;
    /** Keys to _vectors with their index.*/
    private readonly String[] _indexes;
    private readonly Options _options;

    /** Datastructure for holding K largest values.
     * @param TL key in heap
     */
    public class KHighest<TL> where TL : notnull {
        private PriorityQueue<TL, double> _heap = new PriorityQueue<TL, double>();
        private Dictionary<TL, double> _priorities = new Dictionary<TL, double>();
        
        /** Maximum capacity the data structure will hold. */
        private readonly int _capacity;
        
        /** Number of elements in the DS. */
        public int count => _heap.Count;

        /*
         * @param capacity The size of this DS will not exceed this size
         */
        public KHighest(int capacity = int.MaxValue) {
            this._capacity = capacity;
        }

        /** Adds element.
         * @param element Key in heap
         * @param priority How important the element is
         */
        public void enqueue(TL element, double priority) {
            _priorities.Add(element, priority);
            _heap.Enqueue(element, priority);
            while (_heap.Count > _capacity) {
                dequeue();
            }
        }

        /** Pops element from the DS.
         * @return HeapEntry with the element, and the original priority given
         */
        public HeapEntry<TL>? dequeue() {
            if (count == 0) return null;
            TL element = _heap.Dequeue();
            double priority = _priorities[element];
            _priorities.Remove(element);
            return new HeapEntry<TL>(element, priority);
        }
    }

    public Closest(ConcurrentDictionary<String, Dictionary<TK, int>> vectors, Options options) {
        _vectors = vectors;
        _indexes = vectors.Keys.ToArray();
        _options = options;
    }

    /** Finds k vectors which are closest to each other
     * Heap returns them in reverse order
     * @param k How many to return
     * @param from Which vectors are going to be processed on this call
     * @param to Which vectors are going to be processed on this call
     * @return Pairs of Keys to vectors, which were found to be the closest
     */
    public KHighest<String[]> getKClosest(int k, int from, int to) {
        if (from < 0 || to >= _indexes.Length) {
            Console.Error.WriteLine("Range shrank to be inside the array");
            from = Math.Max(from, 0);
            to = Math.Min(_indexes.Length - 1, to);
        }

        KHighest<String[]> kClosest = new KHighest<String[]>(_options.pairsToFind);
        for (int i = from; i <= to; i++) {
            for (int j = i + 1; j <= to; j++) {
                kClosest.enqueue(
                    [_indexes[i], _indexes[j]],
                    cosineSimilarity(_vectors[_indexes[i]], _vectors[_indexes[j]]));
            }
        }

        return kClosest;
    }
   

    /** Helper function to merge heaps returned from getKClosest recursive calls
     * @param heap1 Heap to be merged
     * @param heap2 Heap to be merged
     * @return Merged heap
     */
    private KHighest<String[]> mergeHeaps(KHighest<String[]> heap1, KHighest<String[]> heap2) {
        KHighest<String[]> final = new KHighest<String[]>(_options.pairsToFind);
        while (heap1.count > 0) {
            HeapEntry<String[]>? entry = heap1.dequeue();
            if (entry == null) continue; // This path should never happen
            final.enqueue(entry.Key, entry.Priority);
        }

        while (heap2.count > 0) {
            HeapEntry<String[]>? entry = heap2.dequeue();
            if (entry == null) continue; // This path will not should never happen
            final.enqueue(entry.Key, entry.Priority);
        }

        return final;
    }

    /** Determine how similar vectors are based on the angle between them
     * The higher the return value, the more similar they are
     * @param v1 First vector
     * @param v2 Second vector
     * @return cosine of the angle between them
     */
    public static double cosineSimilarity(Dictionary<TK, int> v1, Dictionary<TK, int> v2) {
        HashSet<TK> keys = new HashSet<TK>(v1.Keys);
        keys.UnionWith(v2.Keys);
        long dotProduct = 0;
        long magV1 = 0;
        long magV2 = 0;
        int elem1;
        int elem2;

        foreach (TK key in keys) {
            elem1 = v1.ContainsKey(key) ? v1[key] : 0;
            elem2 = v2.ContainsKey(key) ? v2[key] : 0;
            dotProduct += elem1 * elem2;
            magV1 += elem1 * elem1;
            magV2 += elem2 * elem2;
        }

        double similarity = dotProduct / double.Sqrt(magV1 * magV2);
        return similarity;
    }
}
