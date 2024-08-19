namespace TwinFinder.ContentFinding;

/** Finds files matching patterns */
public class FilesFinder : IContentFinder {
    private static String _currentPath = ".";

    /** Finds files in a given path, which match the pattern provided
     * @param path Where to look for files
     * @param patterns Patterns to match to find files
     * @return List of all files matching the description provided
     */
    public String[] find(String path, String[] patterns) {
        _currentPath = path;
        
        List<string> files = new List<string>();
        foreach (String pattern in patterns) {
            if (Path.IsPathRooted(pattern)) files.Add(pattern);
            else files.AddRange(Directory.GetFiles(_currentPath, pattern));
        }
        String[] result = files.Where(file => File.Exists(file)).ToArray();
        return result;
    }
}