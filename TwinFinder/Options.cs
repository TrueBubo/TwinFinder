namespace TwinFinder;

public class Options {
    
    // Default settings 
    
    private String _mode = "closest";
    public String mode {
        get { return _mode; }
        set { _mode = value; }
    }

    private bool _normalizeWords = false;
    public bool normalizeWords {
        get { return _normalizeWords; }
        set { _normalizeWords = value;  }
    }

    private int _pairsToFind = 3;
    public int pairsToFind {
        get { return _pairsToFind; }
        set { _pairsToFind = value; }
    }

    private int _synonymCount = 5;

    public int synonymCount {
        get { return _synonymCount; }
        set { _synonymCount = value; }
    }

    public bool isValid() {
        return (pairsToFind >= 0 && synonymCount >= 0);
    }

}