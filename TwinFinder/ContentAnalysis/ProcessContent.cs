using System.Collections.Concurrent;
using TwinFinder.Configuration;
using TwinFinder.ContentIO;

namespace TwinFinder.ContentAnalysis;

/** DS for bundling Key with Priority */
public class HeapEntry<TL> {
    public readonly TL Key;
    public readonly double Priority;

    public HeapEntry(TL key, double priority) {
        Key = key;
        Priority = priority;
    }
}

/** Processes Content based on options set */
public class ProcessContent {
    /** Frequencies of words in a given content
     * (Content, (Word, Frequency))
     */
    private ConcurrentDictionary<String, Dictionary<String, int>> _frequencies =
        new ConcurrentDictionary<String, Dictionary<String, int>>();

    /** Unique words across all the contents */
    private ConcurrentBag<String> _uniqueWords = new ConcurrentBag<String>();
    private Synonyms _synonyms;

    public ProcessContent(Options options, Synonyms synonyms) {
        _synonyms = synonyms;
        switch (options.mode) {
            case Options.Mode.Closest:
                break;
            default:
                Console.Error.WriteLine($"Mode {options.mode} does not exist");
                Environment.Exit(1);
                break;
        }
    }

    /** Process individual contents
     * @param loc Location of the content
     * @param parser Parser for getting content from the given location
     */
    public void processContent(String loc, IWordsParser parser, Options options) {
        String[] words = parser.parse(loc, options.normalizeWords);

        switch (options.mode) {
            case Options.Mode.Closest: {
                processClosest(loc, words);
                break;
            }
            default: {
                Console.Error.WriteLine($"Mode {options.mode} does not exist");
                Environment.Exit(1);
                break;
            }
        }
    }

	private void processClosest(String loc, String[] words) {
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
	}
    
    /** Gets similar contents based on options given
     * @return Most similar contents
     */
    public HeapEntry<String[]>[] getTwinFiles(Options options) {
        switch (options.mode) {
            case Options.Mode.Closest:
				return getTwinFilesClosest(options);

            default:
                Console.Error.WriteLine($"Mode {options.mode} does not exist");
                Environment.Exit(1);
                return null;
        }
    }

	private HeapEntry<String[]>[] getTwinFilesClosest(Options options) {
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
	}

    /** Pads frequencies with 0s */
    public void padFrequencies(String loc) {
        foreach (string word in _uniqueWords) {
            if (!_frequencies[loc].ContainsKey(word)) _frequencies[loc][word] = 0;
        }
    }
}