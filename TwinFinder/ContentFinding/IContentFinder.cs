using System.Runtime.CompilerServices;

namespace TwinFinder.ContentFinding;

public interface IContentFinder {
    // Finds locations satisfying given pattern
    public String[] find(String[] patterns);
}