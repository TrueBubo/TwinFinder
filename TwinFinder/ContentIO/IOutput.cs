namespace TwinFinder.ContentIO;

/** Interface defining methods used for outputting */ 
public interface IOutput {
    /** Writes output to the selected device
     * @param output String to be printed to the device
     */
    public void WriteLine(String output);
}