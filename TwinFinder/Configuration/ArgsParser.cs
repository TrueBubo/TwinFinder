using System.Collections;

namespace TwinFinder.Configuration;

public class ArgsParser {
    // cmd call: name in OptionsParser
    private Dictionary<String, String> _validOptions =
        new Dictionary<String, String>() {
            {"-m", "mode"},
            {"--normalize", "normalizeWords"},
            {"-n", "pairsToFind"},
            {"-s", "synonymCount"},
            {"--lang", "language"},
            {"--absolute", "useAbsolutePaths"}
        };
    
    public class Args {
        // Same option names as in OptionsParser
        private Hashtable _options;
        public Hashtable options => _options;

        // For placements of contents to be compared
        private String[] _contentLocations;
        public String[] contentLocations => _contentLocations;
        
        public Args(Hashtable options, String[] contentLocations) {
            _options = options;
            _contentLocations = contentLocations;
        }
    }

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