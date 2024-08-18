﻿using TwinFinder.Configuration;
using TwinFinder.ContentAnalysis;
using TwinFinder.ContentFinding;
using TwinFinder.ContentIO;

namespace TwinFinder;

internal class Program {
    static void Main(string[] args) {
        const String projectName = "TwinFinder";
        const String configName = "config.toml";
        
        String cwd = Environment.CurrentDirectory; // Where the program was called from
        String basePath = AppDomain.CurrentDomain.BaseDirectory;
        Directory.SetCurrentDirectory(basePath);

        String configLoc = createConfig(configName, projectName);
        
        IConfigReader reader = new TomlConfigReader();
        OptionsParser optionsParser = new OptionsParser(configLoc, args, reader);
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

    /** Creates user editable config in the system config directory
     * @param configName How is the config file called
     * @param projectName Short name for the project to be used as a directory name
     * @return Location of the config file in the system config directory
     */
    private static String createConfig(String configName, String projectName) {
        String configs = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        String configLoc = Path.Combine(configs, projectName, configName);

        if (File.Exists(configLoc)) return configLoc;
        Directory.CreateDirectory(Path.GetDirectoryName(configLoc) ?? ".");
        File.Copy(configName, configLoc);
        return configLoc;
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