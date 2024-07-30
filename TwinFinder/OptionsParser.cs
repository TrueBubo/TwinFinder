using System.Collections;

namespace TwinFinder;

public class OptionsParser {
   private static OptionsParser? _instance; // Singleton
   private static readonly object _lock = new object(); // Prevents race condition
   
   
   // Default settings
   private Hashtable _options = new Hashtable() {
      {"mode", "closest"},
      {"normalizeWords", false},
      {"pairsToFind", 3},
      {"synonymCount", 5}
   };

   public Hashtable options {
      get { return _options; }
   }
   

   public OptionsParser(String filename, IConfigReader configReader) {
      lock (_lock) {
         if (_instance != null) return;
         _instance = this;
         
         // Loads settings from config file
         Hashtable configOptions = configReader.parse(filename);
         foreach (var key in configOptions.Keys) {
            _options[key] = configOptions[key];
         }
      }
   }
}