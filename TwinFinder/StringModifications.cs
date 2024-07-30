using System.Globalization;
using System.Text;
namespace TwinFinder;

public static class StringModifications {
    public static String getAlphabeticalPart(String word) {
            return new String(word.Where(char.IsLetter).ToArray());
        }
     
    // Standardizes text to the form without any diacritics. Texts without diacritics will be therefore be
    // similar to the same text with diacritics.
    // Converts čau to cau 
    // Warning, by using this function you risk two unrelated words colliding e.g bít (to beat), bit (binary digit)
    // Taken from here: https://www.levibotelho.com/development/c-remove-diacritics-accents-from-a-string/
    public static string removeDiacritics(string text) {
        if (string.IsNullOrWhiteSpace(text))
            return text;
    
        text = text.Normalize(NormalizationForm.FormD);
        var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
        return new string(chars).Normalize(NormalizationForm.FormC);
    }
}