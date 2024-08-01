using TwinFinder.Configuration;
using TwinFinder.ContentAnalysis;
using TwinFinder.ContentFinding;
using TwinFinder.ContentIO;

namespace TwinFinder;

internal class Program {
	static void Main(string[] args) {
		String basePath = AppDomain.CurrentDomain.BaseDirectory;
		basePath = Path.Combine(basePath, "../../.."); // Go to directory where .cs files are located
		Directory.SetCurrentDirectory(basePath);

		String configName = "config.toml";
		IConfigReader reader = new TomlConfigReader();
		OptionsParser optionsParser = new OptionsParser(configName, args, reader);
		Options options = optionsParser.options;

		IContentFinder contentFinder = new FilesFinder();
		String[] files = contentFinder.find(args);

		ProcessContent processContent = new ProcessContent();
		Thread[] threads = new Thread[files.Length];

		for (int idx = 0; idx < files.Length; idx++) {
			int localIdx = idx;
			threads[localIdx] = new Thread(
				() => {
					try {
						processContent.processContent(files[localIdx], new FileWordsParser(), options.mode,
							options.normalizeWords);
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
		
		
	}
}




