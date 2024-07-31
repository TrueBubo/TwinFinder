namespace TwinFinder.Configuration;

public class Options {

    // Default settings 

    private String _mode = "closest";

    public String mode {
        get => _mode;
        set => _mode = value;
    }

    private bool _normalizeWords = false;

    public bool normalizeWords {
        get => _normalizeWords;
        set => _normalizeWords = value;
    }

    private int _pairsToFind = 3;

    public int pairsToFind {
        get => _pairsToFind;
        set => _pairsToFind = value;
    }

    private int _synonymCount = 5;

    public int synonymCount {
        get => _synonymCount;
        set => _synonymCount = value;
    }

    public bool isValid() {
        return (pairsToFind >= 0 && synonymCount >= 0);
    }
}


