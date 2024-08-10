namespace TwinFinder.ContentIO;

/** Writes to files */
public class FileOutput : IOutput {
    private readonly StreamWriter _writer;

    public FileOutput(String filename) {
        _writer = new StreamWriter(filename);
    }

    public void WriteLine(String output) {
        _writer.WriteLine(output);
        _writer.Flush();
    }
}