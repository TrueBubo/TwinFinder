using System.Collections.Concurrent;
using TwinFinder.Configuration;
using TwinFinder.ContentIO;

namespace TwinFinder.ContentAnalysis;

public class HeapEntry<TL> {
    public readonly TL Key;
    public readonly double Priority;
    public HeapEntry(TL key, double priority) {
        Key = key;
        Priority = priority;
    }
}
public class ProcessContent {
    private ConcurrentDictionary<String, Dictionary<String, int>> _frequencies =
        new ConcurrentDictionary<String, Dictionary<String, int>>();

    private ConcurrentBag<String> _uniqueWords = new ConcurrentBag<String>();
    private Synonyms _synonyms = new Synonyms();

    public ProcessContent(Options options) {
        switch (options.mode) {
            case "closest":
                _synonyms = new Synonyms(options.language, options.synonymCount);
                break;
            default:
                Console.Error.WriteLine($"Mode {options.mode} does not exist");
                Environment.Exit(1);
                break;
        }
    }

    public void processContent(String loc, IWordsParser parser, Options options) {
        String[] words = parser.parse(loc, options.normalizeWords);

        switch (options.mode) {
            case "closest": {
                List<String> wordList = words.ToList();
                foreach (String word in words) {
                    foreach (String synonym in _synonyms.get(word)) {
                        wordList.Add(synonym);
                    }
                }

                Dictionary<String, int> frequenciesFile = TextStats.getFrequencies(wordList.ToArray());
                _frequencies[loc] = frequenciesFile;
                foreach (String word in frequenciesFile.Keys) {
                    _uniqueWords.Add(word);
                }

                
                break;
            }
            default: {
                Console.Error.WriteLine($"Mode {options.mode} does not exist");
                Environment.Exit(1);
                break;
            }
        }
    }

    public HeapEntry<String[]>[] getTwinFiles(Options options) {
        switch (options.mode) {
            case "closest":
                Closest<String> closest = new Closest<String>(_frequencies, options);
                var result = closest.getKClosest(options.pairsToFind, 0, _frequencies.Count - 1);
                
                List<HeapEntry<String[]>> entries = new List<HeapEntry<String[]>>();
                HeapEntry<String[]>? entry = result.dequeue();
                while (entry != null) {
                    entries.Add(entry);
                    entry = result.dequeue();
                }

                entries.Reverse();
                return entries.ToArray();
                
            default:
                Console.Error.WriteLine($"Mode {options.mode} does not exist");
                Environment.Exit(1);
                return null;
        }
    }

    // Pads frequencies with 0s, so every file contains the same keys to make comparing easier
    public void padFrequencies(String loc) {
        foreach (string word in _uniqueWords) {
            if (!_frequencies[loc].ContainsKey(word)) _frequencies[loc][word] = 0;
        }
    }
}