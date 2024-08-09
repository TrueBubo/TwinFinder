using System.Collections;

namespace TwinFinder.Configuration;

/** Interface defining methods used for reading configs */ 
public interface IConfigReader {
    public Hashtable parse(String loc);
}