//! Handles reading and writing to std and another sources and destinations
namespace TwinFinder.ContentIO;

/** Writes to standard output */
public class ConsoleOutput : IOutput {
    public void WriteLine(String output) {
        Console.WriteLine(output);
    }
}
