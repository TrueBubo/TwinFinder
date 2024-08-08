using System.Resources;

namespace TwinFinder.Configuration;

public class Options {
    // Default settings 
    public enum Mode {
        Closest
    }
    
    private static Dictionary<String, Mode> _modes = new Dictionary<String, Mode>() {
        { "closest", Mode.Closest }
    };
    

    private Mode _mode = Defaults.Mode;

    public Mode mode {
        get => _mode;
        set => _mode = value;
    }

    public bool setMode(String strMode) {
        if (!_modes.ContainsKey(strMode)) {return false;}
        mode = _modes[strMode];
        return true;
    }

    private bool _normalizeWords = Defaults.NormalizeWords;

    public bool normalizeWords {
        get => _normalizeWords;
        set => _normalizeWords = value;
    }

    private int _pairsToFind = Defaults.PairsToFind;

    public int pairsToFind {
        get => _pairsToFind;
        set => _pairsToFind = value;
    }

    private int _synonymCount = Defaults.SynonymCount;

    public int synonymCount {
        get => _synonymCount;
        set => _synonymCount = value;
    }

    public enum Language {
        English
    }
    private static Dictionary<String, Language> _languages = new Dictionary<String, Language>() {
            { "en", Language.English }
    };

    public static readonly Dictionary<Language, String> langToCode = new Dictionary<Language, string>() {
        { Language.English, "en" }
    };
     
    private Language _language = Defaults.Language;

    public Language language {
        get => _language;
        set => _language = value;
    }

    public bool setLanguage(String langCode) {
        if (!_languages.ContainsKey(langCode)) return false;
        language = _languages[langCode];
        return true;
    }
    
    private bool _useAbsolutePaths = Defaults.UseAbsolutePaths;

    public bool useAbsolutePaths {
        get => _useAbsolutePaths;
        set => _useAbsolutePaths = value;
    }

    public bool isValid() {
        return (pairsToFind >= 0 && synonymCount >= 0);
    }
}