namespace TwinFinder.Configuration;

/** Dataclass holding all options.
 * Values set at the start are default values.
 */
public class Options {
    /** Possible values for Mode. */
    public enum Mode {
        Closest
    }
    
    /** Converts string values into Mode enum. */
    private static Dictionary<String, Mode> _modes = new Dictionary<String, Mode>() {
        { "closest", Mode.Closest }
    };

    /** Metric to determine how similar given contents are. */
    private Mode _mode = Defaults.Mode;

    public Mode mode {
        get => _mode;
        set => _mode = value;
    }

    /** Sets mode from a string.
     * @param strMode Mode written as a String. It is a key in _modes
     * @return Success
    */
    public bool setMode(String strMode) {
        if (!_modes.ContainsKey(strMode)) {return false;}
        mode = _modes[strMode];
        return true;
    }

    /** Removes accent and diacritics. Puts the word into the standart form.
     * How normalization is done is determined by the language currently set
     */
    private bool _normalizeWords = Defaults.NormalizeWords;

    public bool normalizeWords {
        get => _normalizeWords;
        set => _normalizeWords = value;
    }

    /** Sets how many closest pairs are returned as an output. */
    private int _pairsToFind = Defaults.PairsToFind;
    
    public int pairsToFind {
        get => _pairsToFind;
        set => _pairsToFind = value;
    }
    
    /** Used for finding words similar in meaning to match similar contents more often. */
    private int _synonymCount = Defaults.SynonymCount;

    public int synonymCount {
        get => _synonymCount;
        set => _synonymCount = value;
    }

    /** Possible values for Language. */
    public enum Language {
        English
    }
    
    /** Converts string values into Language enum. */
    private static Dictionary<String, Language> _languages = new Dictionary<String, Language>() {
            { "en", Language.English }
    };

    /** Convert Language enum back to String.
     * Shows languages in human readable form.
     */
    public static readonly Dictionary<Language, String> langToCode = new Dictionary<Language, string>() {
        { Language.English, "en" }
    };
     
    /** Language the content searched is in.
     * Helps with context.
     */
    private Language _language = Defaults.Language;

    public Language language {
        get => _language;
        set => _language = value;
    }

    /**
     * Sets language from String.
     * @param langCode Language code as per ISO 639. It is a key in _languages.
     * @return Success.
     */
    public bool setLanguage(String langCode) {
        if (!_languages.ContainsKey(langCode)) return false;
        language = _languages[langCode];
        return true;
    }
    
    /** Use full paths instead of relative.
     * - If set to true full paths will be used.
     * - If set to false relative paths to the current working directory will be shown.
     */
    private bool _useAbsolutePaths = Defaults.UseAbsolutePaths;

    public bool useAbsolutePaths {
        get => _useAbsolutePaths;
        set => _useAbsolutePaths = value;
    }
}