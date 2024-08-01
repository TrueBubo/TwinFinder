namespace TwinFinder.ContentIO;

public interface IWordsParser {
   // Returns list of words from contents in a given location. Words exclude punctuation
   public String[] parse(String loc, bool withDiacritics); 
}