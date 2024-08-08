using System.Collections;
using System.Diagnostics;

namespace TwinFinder.Configuration;

public class OptionsParser {
   private static OptionsParser? _instance; // Singleton
   private static readonly object Lock = new object(); // Prevents race condition


   
   // Default settings
   private Options _options = new Options();
   public Options options => _options;

   private String[]? _contentLoc;

   public String[]? contentLoc => _contentLoc;

   private static readonly Dictionary<string, Action<Hashtable, string, Options>> OptionSettingFunctions =
      new Dictionary<string, Action<Hashtable, string, Options>> {
         { "mode", (table, nameInTable, options) => options.mode = table[nameInTable]?.ToString() ?? string.Empty }, {
            "normalizeWords",
            (table, nameInTable, options) =>
               options.normalizeWords = bool.Parse(table[nameInTable]?.ToString() ?? "false")
         }, {
            "pairsToFind",
            (table, nameInTable, options) => options.pairsToFind = int.Parse(table[nameInTable]?.ToString() ?? "-1")
         }, {
            "synonymCount",
            (table, nameInTable, options) => options.synonymCount = int.Parse(table[nameInTable]?.ToString() ?? "-1")
         }, {
            "language",
            (table, nameInTable, options) => options.language = table[nameInTable]?.ToString() ?? string.Empty
         }, {"useAbsolutePaths",
         (table, nameInTable, options) => options.useAbsolutePaths = bool.Parse(table[nameInTable]?.ToString() ?? "false")
         }
      };
   
   public OptionsParser(String loc, String[] args, IConfigReader configReader) {
      lock (Lock) {
         if (_instance != null) return; // Ensures there cannot be more instances
         _instance = this;
         
         // Loads settings from config file
         Hashtable configOptions = configReader.parse(loc);
         setOptions(configOptions); 
         // Ensures we do not continue with an invalid config
         if (!options.isValid()) {
            Console.Error.WriteLine($"There is a mistake in {loc}, please before continuing fix the config file");
            Environment.Exit(1);
         }

         ArgsParser.Args cmdOptions = new ArgsParser().parse(args);
         _contentLoc = cmdOptions.contentLocations;
         setOptions(cmdOptions.options);
         if (!options.isValid()) {
            Console.Error.WriteLine($"Program arguments you have entered are not valid");
            Environment.Exit(1);
         }
      }
   }

   private bool setOption(Hashtable configOption, String nameInTable, Action<Hashtable, String, Options> update) {
      if (!configOption.ContainsKey(nameInTable)) return false;
      update(configOption, nameInTable, _options);
      return true;
   }
   
   private void setOptions(Hashtable configOptions) {
      foreach (var pair in OptionSettingFunctions) {
         if (setOption(configOptions, pair.Key, pair.Value)) configOptions.Remove(pair.Key);
      }

      foreach (String key in configOptions.Keys) {
         Console.Error.WriteLine($"Unknown option {key}");
      }
      if (configOptions.Count > 0) Environment.Exit(1);
   }
}