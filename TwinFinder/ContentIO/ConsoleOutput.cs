namespace TwinFinder.ContentIO;

/** Writes to standard output */
public class ConsoleOutput : IOutput {
    public void WriteLine(String output) {
        Console.WriteLine(output);
    }
}