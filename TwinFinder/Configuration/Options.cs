namespace TwinFinder.Configuration;

public class Options {

    // Default settings 
    private String[] _possibleModes = { "closest" };
    private String _mode = Defaults.Mode;

    public String mode {
        get => _mode;
        set => _mode = value;
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

    public bool isValid() {
        return (_possibleModes.Contains(mode) && pairsToFind >= 0 && synonymCount >= 0);
    }
}


