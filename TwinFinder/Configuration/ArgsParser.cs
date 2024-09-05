using System.Collections;

//! Handles everything regarding options. Their parsing, reading and values
namespace TwinFinder.Configuration;

/**
 * Handles arguments given from the command line.
 * The object will contains Hashtable with set options, and locations of files given.
 * Files are automatically parsed from patterns by the user's shell.
 */
public class ArgsParser {
    public class ArgEntry {
        public String flag;
        public String optionName;
        public bool expectsValue;

        public ArgEntry(String flag, String optionName, bool expectsValue) {
            this.flag = flag;
            this.optionName = optionName;
            this.expectsValue = expectsValue;
        }
    }
    
    /** (Flag, Name in OptionsParser).
     * - Key in dictionary corresponds to the flag to be used in the terminal.
     * - Value is the name in OptionsParser
     */
    private Dictionary<String, ArgEntry> _validOptions =
        new Dictionary<String, ArgEntry>() {
            {"-h", new ArgEntry("-h", "help", false)},
            {"m", new ArgEntry("-m", "mode", true)},
            {"--normalize", new ArgEntry("--normalize", "normalizeWords", false)},
            {"-n", new ArgEntry("-n", "pairsToFind", true)},
            {"-s", new ArgEntry("-s", "synonymCount", true)},
            {"--lang", new ArgEntry("--lang", "language", true)},
            {"--absolute", new ArgEntry("--absolute", "useAbsolutePaths", false)},
            {"-o", new ArgEntry("-o", "output", true)}
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
        for (; idx < args.Length; idx++) {
            if (args[idx][0] != '-') break;
            if (!_validOptions.ContainsKey(args[idx])) break;
            ArgEntry argEntry = _validOptions[args[idx]];
            options[argEntry.optionName] = (argEntry.expectsValue) ? 
                ((idx != args.Length - 1) ? args[++idx] : null) : true;
        }

        String[] contentLocations = args[idx..];
        return new Args(options, contentLocations);
    }
}
