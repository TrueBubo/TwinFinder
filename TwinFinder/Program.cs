using TwinFinder.Configuration;
using TwinFinder.ContentAnalysis;
using TwinFinder.ContentFinding;
using TwinFinder.ContentIO;

namespace TwinFinder;

internal class Program {
    static void Main(string[] args) {
        String cwd = Environment.CurrentDirectory; // Where the program was called from

        String basePath = AppDomain.CurrentDomain.BaseDirectory;
        basePath = Path.Combine(basePath, "../../.."); // Go to directory where .cs files are located
        Directory.SetCurrentDirectory(basePath);


        const String configName = "config.toml";
        IConfigReader reader = new TomlConfigReader();
        OptionsParser optionsParser = new OptionsParser(configName, args, reader);
        Options options = optionsParser.options;

        IContentFinder contentFinder = new FilesFinder();
        String[] files = contentFinder.find(cwd, args);

        ProcessContent processContent = new ProcessContent(options);
        Thread[] threads = new Thread[files.Length];

        for (int idx = 0; idx < files.Length; idx++) {
            int localIdx = idx;
            threads[localIdx] = new Thread(
                () => {
                    try {
                        processContent.processContent(files[localIdx], new FileWordsParser(), options);
                    }
                    catch (Exception e) {
                        Console.Error.WriteLine($"{files[localIdx]} could not be processed");
                        Console.Error.WriteLine(e);
                    }
                });
            threads[localIdx].Start();
        }

        // Waiting for all threads to finish
        foreach (Thread thread in threads) {
            thread.Join();
        }

        IOutput output = (options.outputFile != "") ? new FileOutput(options.outputFile) : new ConsoleOutput();
        const int decimalPrecision = 4;
        foreach (HeapEntry<String[]> entry in processContent.getTwinFiles(options)) {
            output.WriteLine(formatSimilar(entry, options, decimalPrecision, cwd));
        }
    }

    private static String formatSimilar(HeapEntry<String[]> entry, Options options, int decimalPrecision, string cwd) {
        // Key refers to the paths of files compared, priority is their similarity
        String path1 = options.useAbsolutePaths
            ? Path.GetFullPath(entry.Key[0])
            : Path.GetRelativePath(cwd, entry.Key[0]);
        String path2 = options.useAbsolutePaths
            ? Path.GetFullPath(entry.Key[1])
            : Path.GetRelativePath(cwd, entry.Key[1]);
        double similarity = Math.Round(entry.Priority, decimalPrecision); // To not display really long double
        return entry.Priority != 0 ? ($"{path1} {path2} {similarity}") : "";
    }
}