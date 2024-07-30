using System.Collections;

namespace TwinFinder.Configuration;

public class OptionsParser {
   private static OptionsParser? _instance; // Singleton
   private static readonly object Lock = new object(); // Prevents race condition
   
   
   // Default settings
   private Options _options = new Options();
   public Options options {
      get { return _options; }
   }
   

   public OptionsParser(String filename, IConfigReader configReader) {
      lock (Lock) {
         if (_instance != null) return; // Ensures there cannot be more instances
         _instance = this;
         
         // Loads settings from config file
         Hashtable configOptions = configReader.parse(filename);
         
         try {
            if (configOptions.ContainsKey("mode")) _options.mode = configOptions["mode"]?.ToString() ?? String.Empty;
            if (configOptions.ContainsKey("normalizeWords"))
               _options.normalizeWords =
                  bool.Parse(configOptions["normalizeWords"]?.ToString() ?? "false");
            if (configOptions.ContainsKey("pairsToFind"))
               _options.pairsToFind = int.Parse(configOptions["pairsToFind"]?.ToString() ?? "-1");
            if (configOptions.ContainsKey("synonymCount"))
               _options.synonymCount = int.Parse(configOptions["synonymCount"]?.ToString() ?? "-1");
            if (!options.isValid()) throw new ArgumentException(); // Ensures we do not continue with an invalid config
         }
         catch (ArgumentException) {
            Console.Error.WriteLine($"There is a mistake in {filename}, please before continuing fix the config file");
            Environment.Exit(1);
         }
      }
   }
}