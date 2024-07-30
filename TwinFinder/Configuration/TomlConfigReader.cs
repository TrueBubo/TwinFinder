using System.Collections;

namespace TwinFinder.Configuration;

using Tommy;

public class TomlConfigReader : IConfigReader {
    public Hashtable parse(String filename) {
        StreamReader reader = File.OpenText(filename);
        Hashtable table = toHashtable(TOML.Parse(reader));
        return table;
    }

    private Hashtable toHashtable(TomlTable table) {
        Hashtable hashtable = new Hashtable();
        foreach (var key in table.Keys) {
            hashtable.Add(key, table[key]);
        }

        return hashtable;
    }
}