namespace TwinFinder.ContentFinding;

public class FilesFinder : IContentFinder {
    private static String _currentPath = ".";

    public String[] find(String path, String[] patterns) {
        _currentPath = path;
        
        List<string> files = new List<string>();
        foreach (String pattern in patterns) {
               files.AddRange(Directory.GetFiles(_currentPath, pattern));
        }
        String[] result = files.Where(file => File.Exists(file)).ToArray();

        return result;
    }
}