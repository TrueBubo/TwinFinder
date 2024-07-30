using System.Collections;

namespace TwinFinder.Configuration;

public interface IConfigReader {
    public Hashtable parse(String filename);
}