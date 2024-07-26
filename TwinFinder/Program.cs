using System.Collections;

namespace TwinFinder;

internal class Program {
	static void Main(string[] args) {
		String basePath = AppDomain.CurrentDomain.BaseDirectory;
		basePath = Path.Combine(basePath, "../../.."); // Go to directory where .cs files are located
		String configName = "config.toml";
		
		TomlConfigReader reader = new TomlConfigReader();
		OptionsParser optionsParser = new OptionsParser(Path.Combine(basePath, configName), reader);
		Hashtable options = optionsParser.options;
		
		
	}
}




