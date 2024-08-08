namespace TwinFinder.Configuration;

static class Defaults {
    public const Options.Mode Mode = Options.Mode.Closest;
    public const bool NormalizeWords = false;
    public const int PairsToFind = 3;
    public const int SynonymCount = 5;
    public const Options.Language Language = Options.Language.English;
    public const bool UseAbsolutePaths = false;
}