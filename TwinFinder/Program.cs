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
		OptionsParser optionsParser = new OptionsParser(configName, reader);
		Options options = optionsParser.options;
		
		IContentFinder contentFinder = new FilesFinder();
		String[] files = contentFinder.find(args);
		
		ProcessContent processContent = new ProcessContent();
		foreach(String filename in files) {
			Thread thread = new Thread(
				() => {
					try {
						processContent.processContent(filename, new FileWordsParser(), options.mode, options.normalizeWords);
					}
					catch (Exception e) {
						Console.Error.WriteLine($"{filename} could not be processed");
						Console.Error.WriteLine(e);
					}
				});
			thread.Start();
		}
	}
}




