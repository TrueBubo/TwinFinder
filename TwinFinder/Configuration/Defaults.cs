namespace TwinFinder.Configuration;

/** Provides default values for various options used in the application.
 * This static class contains default values for all possible options. If an options was not set
 * by the user the value in this class will be used.
 */
static class Defaults {
    public const Options.Mode Mode = Options.Mode.Closest;
    public const bool NormalizeWords = false;
    public const int PairsToFind = 3;
    public const int SynonymCount = 5;
    public const Options.Language Language = Options.Language.English;
    public const bool UseAbsolutePaths = false;
    public const String OutputFile = "";
}