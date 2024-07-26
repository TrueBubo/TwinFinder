using System.Collections;

namespace TwinFinder;

public interface IConfigReader {
    public Hashtable parse(String filename);
}