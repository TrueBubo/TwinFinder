using System.Collections;

namespace TwinFinder.Configuration;

/**
 * Handles arguments given from the command line.
 * The object will contains Hashtable with set options, and locations of files given.
 * Files are automatically parsed from patterns by the user's shell.
 */
public class ArgsParser {
    /** (Flag, Name in OptionsParser).
     * - Key in dictionary corresponds to the flag to be used in the terminal.
     * - Value is the name in OptionsParser
     */
    private Dictionary<String, String> _validOptions =
        new Dictionary<String, String>() {
            { "-h", "help"},
            { "-m", "mode" },
            { "--normalize", "normalizeWords" },
            { "-n", "pairsToFind" },
            { "-s", "synonymCount" },
            { "--lang", "language" },
            { "--absolute", "useAbsolutePaths" },
            { "-o", "output"}
        };

    /** Dataclass holding parsed command line arguments
     */ 
    public class Args {
        /** Hashtable with options: (Name in OptionsParser, Value) */
        private Hashtable _options;
        public Hashtable options => _options;

        /** String[] with full paths to contents designated to be compared */
        private String[] _contentLocations;
        public String[] contentLocations => _contentLocations;
        
        public Args(Hashtable options, String[] contentLocations) {
            _options = options;
            _contentLocations = contentLocations;
        }
    }

    /** Parses command line arguments
     * @param args command line arguments
     * @return Args with set options and content locations
     */
    public Args parse(String[] args) {
        Hashtable options = new Hashtable();
        int idx = 0;
        for (; idx < args.Length - 1; idx++) {
            if (args[idx][0] != '-') break;
            if (!_validOptions.ContainsKey(args[idx])) break;
            options[_validOptions[args[idx]]] = args[++idx];
        }

        String[] contentLocations = args[idx..];
        return new Args(options, contentLocations);
    }
}