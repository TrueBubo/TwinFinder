using System.Collections.Concurrent;
using TwinFinder.Configuration;

namespace TwinFinder.ContentAnalysis;

public class Closest<TK> where TK : notnull {
    private readonly ConcurrentDictionary<String, Dictionary<TK, int>> _vectors;
    private readonly String[] _indexes;
    private readonly Options _options;

    public class KHighest<TL> where TL : notnull {
        private PriorityQueue<TL, double> _heap = new PriorityQueue<TL, double>();
        private Dictionary<TL, double> _priorities = new Dictionary<TL, double>();
        private readonly int _capacity;
        public int count => _heap.Count;
        public KHighest(int capacity = int.MaxValue) {
            this._capacity = capacity;
        }

        public void enqueue(TL element, double priority) {
            _priorities.Add(element, priority);
            _heap.Enqueue(element, -priority);
            while (_heap.Count > _capacity) {
                dequeue();
            }
        }

        public HeapEntry<TL>? dequeue() {
            if (count == 0) return null; 
            TL element = _heap.Dequeue();
            double priority = _priorities[element];
            _priorities.Remove(element);
            return new HeapEntry<TL>(element, -priority);
         }

        
        
    }

    public Closest(ConcurrentDictionary<String, Dictionary<TK, int>> vectors, Options options) {
        _vectors = vectors;
        _indexes = vectors.Keys.ToArray();
        _options = options;

    }

    // Heap returns them in reverse order
    public KHighest<String[]> getKClosest(int k, int from, int to) {
        if (from < 0 || to >= _indexes.Length) {
            Console.Error.WriteLine("Range shrank to be inside the array");
            from = Math.Max(from, 0);
            to = Math.Min(_indexes.Length - 1, to);
        }

        if (to - from <= k) {
            KHighest<String[]> kClosest = new KHighest<String[]>(_options.pairsToFind);
            for (int i = from; i <= to; i++) {
                for (int j = i + 1; j <= to; j++) {
                    kClosest.enqueue(
                        [_indexes[i], _indexes[j]],
                        -cosineSimilarity(_vectors[_indexes[i]], _vectors[_indexes[j]]));
                }
            }

            return kClosest;
        }

        KHighest<String[]> left = new KHighest<String[]>(_options.pairsToFind);
        Thread leftThread = new Thread(
            () => left = getKClosest(k, from, (from + to) / 2)
        );
        leftThread.Start();

        KHighest<String[]> right = new KHighest<String[]>(_options.pairsToFind);
        Thread rightThread = new Thread(
            () => right = getKClosest(k, (from + to) / 2 + 1, to)
        );
        rightThread.Start();

        leftThread.Join();
        rightThread.Join();

        return mergeHeaps(left, right);
    }

    private KHighest<String[]> mergeHeaps(KHighest<String[]> heap1, KHighest<String[]> heap2) {
        KHighest<String[]> final = new KHighest<String[]>(_options.pairsToFind);
        while (heap1.count > 0) {
            HeapEntry<String[]>? entry = heap1.dequeue();
            if (entry == null) continue; // This path will not should never happen
            final.enqueue(entry.Key, entry.Priority);
        }

        while (heap2.count > 0) {
            HeapEntry<String[]>? entry = heap2.dequeue();
            if (entry == null) continue; // This path will not should never happen
            final.enqueue(entry.Key, entry.Priority);
        }

        return final;
    }
    
    static double cosineSimilarity(Dictionary<TK, int> v1, Dictionary<TK, int> v2) {
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

        return dotProduct / double.Sqrt(magV1 * magV2);
    }
}