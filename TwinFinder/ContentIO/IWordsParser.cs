namespace TwinFinder.ContentIO;

public interface IWordsParser {
   // Returns list of words from files excluding punctuation
   public String[] parse(String filename, bool withDiacritics); 
}