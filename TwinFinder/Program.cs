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

		List<string> files = new List<string>();
		foreach (String pattern in args) {
			files.AddRange(Directory.GetFiles(basePath, pattern));
		}

		files = files.Where(file => File.Exists(file)).ToList();
		
		ProcessFiles processFiles = new ProcessFiles();
		foreach(String filename in files) {
			Thread thread = new Thread(
				() => {
					try {
						processFiles.processFile(filename, options.mode, options.normalizeWords);
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




