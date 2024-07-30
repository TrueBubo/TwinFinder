using System.Collections;

namespace TwinFinder;

public class OptionsParser {
   private static OptionsParser? _instance; // Singleton
   private static readonly object _lock = new object(); // Prevents race condition
   
   
   // Default settings
   private Options _options = new Options();
   private Hashtable table = new Hashtable();
   public Options options {
      get { return _options; }
   }
   

   public OptionsParser(String filename, IConfigReader configReader) {
      lock (_lock) {
         if (_instance != null) return;
         _instance = this;
         
         // Loads settings from config file
         Hashtable configOptions = configReader.parse(filename);
         foreach (var key in configOptions.Keys) {
            table[key] = configOptions[key];
         }

         try {
            if (table.ContainsKey("mode")) _options.mode = table["mode"]?.ToString() ?? String.Empty;
            if (table.ContainsKey("normalizeWords"))
               _options.normalizeWords =
                  bool.Parse(table["normalizeWords"]?.ToString() ?? "false");
            if (table.ContainsKey("pairsToFind"))
               _options.pairsToFind = int.Parse(table["pairsToFind"]?.ToString() ?? "-1");
            if (table.ContainsKey("synonymCount"))
               _options.synonymCount = int.Parse(table["synonymCount"]?.ToString() ?? "-1");
            if (!options.isValid()) throw new ArgumentException(); // Ensures we do not continue with an invalid config
         }
         catch (ArgumentException) {
            Console.Error.WriteLine($"There is a mistake in {filename}, please before continuing fix the config file");
            Environment.Exit(1);
         }
      }
   }
}