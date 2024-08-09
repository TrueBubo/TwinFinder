using System.Runtime.CompilerServices;

namespace TwinFinder.ContentFinding;

/** Interface defining methods used for finding content */ 
public interface IContentFinder {
    // Finds locations satisfying given pattern
    public String[] find(String loc, String[] patterns);
}