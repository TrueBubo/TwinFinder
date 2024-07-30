using System.Collections;
using System.Net.Security;

namespace TwinFinder;

internal class Program {
	static void Main(string[] args) {
		String basePath = AppDomain.CurrentDomain.BaseDirectory;
		basePath = Path.Combine(basePath, "../../.."); // Go to directory where .cs files are located
		Directory.SetCurrentDirectory(basePath);

		String configName = "config.toml";
		TomlConfigReader reader = new TomlConfigReader();
		OptionsParser optionsParser = new OptionsParser(configName, reader);
		Options options = optionsParser.options;
		
		FileWordsParser wordsReader = new FileWordsParser();

		ProcessFiles processFiles = new ProcessFiles();
		foreach(String filename in args) {
			Thread thread = new Thread(
				() => processFiles.processFile(filename, options.mode.ToString(), options.normalizeWords)
			);
			thread.Start();
		}
		
		
	}

	
	
}




