using System.Collections;
using TwinFinder.ContentAnalysis;

namespace TwinFinder.Configuration;

/** Class managing parsing of all options given from both the config file and command line arguments
 * 
 */
public class OptionsParser {
    private static OptionsParser? _instance; // Singleton
    private static readonly object Lock = new(); // Prevents race condition


    /** Will store the options set
     * Starts with default settings
     */
    private Options _options = new();
    public Options options => _options;

    /** Location of the contents to be compared */
    private String[]? _contentLoc;

    public String[]? contentLoc => _contentLoc;

    /** Functions used for parsing selected options */
    private static readonly Dictionary<string, Action<Hashtable, string, Options>> OptionSettingFunctions =
        new() {
            { "help", showHelp},
            { "mode", parseMode },
            { "normalizeWords", parseNormalize },
            { "pairsToFind", parsePairsToFind },
            { "synonymCount", parseSynonymCount },
            { "language", parseLanguage },
            { "useAbsolutePaths", parseAbsolutePath },
            { "output", parseOutputFile}
        };

    public OptionsParser(String loc, String[] args, IConfigReader configReader) {
        lock (Lock) {
            if (_instance != null) return; // Ensures there cannot be more instances
            _instance = this;

            // Loads settings from config file
            if (File.Exists(loc)) loadConfigFile(configReader, loc);
            else Console.Error.WriteLine($"Config file {loc} does not exist");

            loadArgs(args);
        }
    }
    
    /** Sets options from config file */
    private void loadConfigFile(IConfigReader configReader, String loc) {
        Hashtable configOptions = configReader.parse(loc);
        setOptions(configOptions);
    }

    /** Sets options from command line arguments */
    private void loadArgs(String[] args) {
        if (args.Length == 0) showHelp();
        ArgsParser.Args cmdOptions = new ArgsParser().parse(args);
        _contentLoc = cmdOptions.contentLocations;
        setOptions(cmdOptions.options);
    }

    /** Sets given option
     * @param configOption Hashtable with options set
     * @param nameInTable How is the option known in configuration
     * @param update Function, which updates options
     * @return Success
     */
    private bool setOption(Hashtable configOption, String nameInTable, Action<Hashtable, String, Options> update) {
        if (!configOption.ContainsKey(nameInTable)) return false;
        update(configOption, nameInTable, _options);
        return true;
    }
    
    /** Sets options parsed
     * @param configOption Hashtable with options set
     */
    private void setOptions(Hashtable configOptions) {
        foreach (var pair in OptionSettingFunctions) {
            if (setOption(configOptions, pair.Key, pair.Value)) configOptions.Remove(pair.Key);
        }

        foreach (String key in configOptions.Keys) {
            Console.Error.WriteLine($"Unknown option {key}");
        }
    }
    
    private static void showHelp(Hashtable arg1, string arg2, Options arg3) {
        showHelp();
    }

    private static void showHelp() {
         String text =
             $"""
             {Project.Name} {Project.Version}
             Released under {Project.License}    
             
             -h          To display this help menu
             -m          What technique will be used to determine similar files (Default: closest)
             --normalize Removes accents and diacritics
             -n          Pairs to find (Default: 3)
             -s          Number of synonyms to consider (Default: 5)
             --lang      Language the texts are in (Default: en)
             --absolute  Prints text locations using absolute paths instead of relative
             -o          Where should the outputs of the program be sent
             """;
         Console.WriteLine(text);
         Environment.Exit(0);       
    }

    private static void parseMode(Hashtable table, String nameInTable, Options options) {
        String enteredMode = table[nameInTable]?.ToString() ?? string.Empty;
        if (options.setMode(enteredMode)) return;
        Console.Error.WriteLine($"Mode {enteredMode} is not valid. The selected mode did not change");
    }

    private static void parseNormalize(Hashtable table, String nameInTable, Options options) {
        String value = table[nameInTable]?.ToString() ?? "false";
        try {
            bool enteredValue = bool.Parse(value);
            options.normalizeWords = enteredValue;
        }
        catch (FormatException) {
            Console.Error.WriteLine(
                $"Could not convert {value} to boolean. The selected option stays the same");
        }
    }

    private static void parsePairsToFind(Hashtable table, String nameInTable, Options options) {
        String value = table[nameInTable]?.ToString() ?? "-1";
        try {
            int enteredValue = int.Parse(value);
            if (enteredValue < 0) return;
            options.pairsToFind = enteredValue;
        }
        catch (FormatException) {
            Console.Error.WriteLine(
                $"Could not convert {value} to integer. The selected option stays the same");
        }
    }

    private static void parseSynonymCount(Hashtable table, String nameInTable, Options options) {
        String value = table[nameInTable]?.ToString() ?? "-1";
        try {
            int enteredValue = int.Parse(value);
            if (enteredValue < 0) return;
            options.synonymCount = int.Parse(value);
        }
        catch (FormatException) {
            Console.Error.WriteLine(
                $"Could not convert {value} to integer. The selected option stays the same");
        }
    }

    private static void parseLanguage(Hashtable table, String nameInTable, Options options) {
        String enteredLang = table[nameInTable]?.ToString() ?? string.Empty;
        if (options.setLanguage(enteredLang)) return;
        Console.Error.WriteLine(Synonyms.langNotFoundMessage(enteredLang));
    }

    private static void parseAbsolutePath(Hashtable table, String nameInTable, Options options) {
        String value = table[nameInTable]?.ToString() ?? "false";
        try {
            bool enteredValue = bool.Parse(value);
            options.useAbsolutePaths = enteredValue;
        }
        catch (FormatException) {
            Console.Error.WriteLine(
                $"Could not convert {value} to boolean. The selected option stays the same");
        }
    }
    
    private static void parseOutputFile(Hashtable table, String nameInTable, Options options) {
            String value = table[nameInTable]?.ToString() ?? "";
            options.outputFile = value;
        }
}