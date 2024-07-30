namespace TwinFinder.ContentFinding;

public class FilesFinder : IContentFinder {
    private static readonly String CurrentPath = ".";

    public String[] find(string[] patterns) {
        List<string> files = new List<string>();
        foreach (String pattern in patterns) {
               files.AddRange(Directory.GetFiles(CurrentPath, pattern));
        }
        String[] result = files.Where(file => File.Exists(file)).ToArray();

        return result;
    }
}