namespace TwinFinder.ContentIO;

/** Interface defining methods used for parsing words */ 
public interface IWordsParser {
   /** Returns list of words from contents in a given location.
    * Words exclude punctuation.
    * @param loc location where to get the words from
    * @param withDiacritics Determines whether to strip the words of diacritic marks
    */
   public String[] parse(String loc, bool withDiacritics); 
}