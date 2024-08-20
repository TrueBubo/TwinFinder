using TwinFinder.Configuration;
using TwinFinder.ContentAnalysis;
using TwinFinder.ContentFinding;
using TwinFinder.ContentIO;

namespace TwinFinder;

internal class Program {
    static void Main(string[] args) {
        String variableName = $"{Project.Name.ToUpper()}_SHARED";
        String? shared = Environment.GetEnvironmentVariable(variableName);
        String cwd = Environment.CurrentDirectory; // Where the program was called from

        String configLoc = createConfig(shared != null ? $"{shared}/{Project.Config}" : Project.Config, Project.Name);

        IConfigReader reader = new TomlConfigReader();
        OptionsParser optionsParser = new OptionsParser(configLoc, args, reader);
        Options options = optionsParser.options;
        options.shared = shared;

        IContentFinder contentFinder = new FilesFinder();
        String[] files = contentFinder.find(cwd, args);

        String synonymsLoc = moveSynonyms(Project.Synonyms, Project.Name);
        Synonyms synonyms = new Synonyms(options.language, options.synonymCount, synonymsLoc);


        ProcessContent processContent = new ProcessContent(options, synonyms);
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

    private static String moveSynonyms(String synonymsName, String projectName) {
        String configs = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        String synonymsLoc = Path.Combine(configs, projectName, synonymsName);

        if (Directory.Exists(synonymsLoc)) return synonymsLoc;
        Directory.CreateDirectory(synonymsLoc);
        String[] files = Directory.GetFiles(synonymsName);

        foreach (String file in files) {
            File.Copy(Path.Combine(synonymsName, Path.GetFileName(file)),
                Path.Combine(synonymsLoc, Path.GetFileName(file)));
        }

        return synonymsLoc;
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
        return $"{path1} {path2} {similarity}";
    }
}