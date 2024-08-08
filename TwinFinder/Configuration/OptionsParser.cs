using System.Collections;

namespace TwinFinder.Configuration;

public class OptionsParser {
    private static OptionsParser? _instance; // Singleton
    private static readonly object Lock = new(); // Prevents race condition


    // Default settings
    private Options _options = new();
    public Options options => _options;

    private String[]? _contentLoc;

    public String[]? contentLoc => _contentLoc;

    private static readonly Dictionary<string, Action<Hashtable, string, Options>> OptionSettingFunctions =
        new() {
            { "mode", parseMode },
            { "normalizeWords", parseNormalize },
            { "pairsToFind", parsePairsToFind },
            { "synonymCount", parseSynonymCount },
            { "language", parseLanguage },
            { "useAbsolutePaths", parseAbsolutePath }
        };

    public OptionsParser(String loc, String[] args, IConfigReader configReader) {
        lock (Lock) {
            if (_instance != null) return; // Ensures there cannot be more instances
            _instance = this;

            // Loads settings from config file
            if (File.Exists(loc)) {
                if (!loadConfigFile(configReader, loc)) Environment.Exit(1);
            }
            else Console.Error.WriteLine($"Config file {loc} does not exist");

            if (!loadArgs(args)) Environment.Exit(1);
        }
    }

    private bool loadConfigFile(IConfigReader configReader, String loc) {
        Hashtable configOptions = configReader.parse(loc);
        setOptions(configOptions);

        // Ensures we do not continue with an invalid config
        if (options.isValid()) return true;
        Console.Error.WriteLine($"There is a mistake in {loc}, please before continuing fix the config file");
        return false;
    }

    private bool loadArgs(String[] args) {
        ArgsParser.Args cmdOptions = new ArgsParser().parse(args);
        _contentLoc = cmdOptions.contentLocations;
        setOptions(cmdOptions.options);
        if (options.isValid()) return true;
        Console.Error.WriteLine($"Program arguments you have entered are not valid");
        return false;
    }

    private bool setOption(Hashtable configOption, String nameInTable, Action<Hashtable, String, Options> update) {
        if (!configOption.ContainsKey(nameInTable)) return false;
        update(configOption, nameInTable, _options);
        return true;
    }

    private void setOptions(Hashtable configOptions) {
        foreach (var pair in OptionSettingFunctions) {
            if (setOption(configOptions, pair.Key, pair.Value)) configOptions.Remove(pair.Key);
        }

        foreach (String key in configOptions.Keys) {
            Console.Error.WriteLine($"Unknown option {key}, exiting...");
        }

        if (configOptions.Count > 0) Environment.Exit(1);
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
        Console.Error.WriteLine(
            $"Language {enteredLang} is not supported. The selected language did not change");
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
}