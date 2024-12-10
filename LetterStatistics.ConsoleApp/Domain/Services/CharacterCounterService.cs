namespace LetterStatistics.ConsoleApp.Domain.Services;

public class CharacterCounterService
{
    /// <summary>
    /// Counts the frequency of each letter in the provided content string.
    /// </summary>
    /// <param name="content">The input string containing the characters to be counted.</param>
    /// <returns>A dictionary where the key is the character, and the value is the frequency of that character in the string.</returns>
    /// <remarks>
    /// This method ignores non-letter characters and counts each letter in a case-insensitive manner.
    /// It converts all characters to lowercase to ensure case-insensitivity in the count.
    /// </remarks>
    public Dictionary<char, int> CountLetters(string content)
    {
        var characterCount = new Dictionary<char, int>();

        // Iterate through each character in the content, only considering letter characters
        foreach (var lowerChar in from ch in content where char.IsLetter(ch) select char.ToLower(ch))
        {
            // Try to add the character to the dictionary, initializing its count to 1 if it's not already present
            if (!characterCount.TryAdd(lowerChar, 1))
            {
                // If the character is already in the dictionary, increment its count
                characterCount[lowerChar]++;
            }
        }

        return characterCount;
    }

    /// <summary>
    /// Prints the frequency of each letter from a given character count dictionary.
    /// </summary>
    /// <param name="characterCount">A dictionary containing the letter counts, where the key is the character and the value is the frequency.</param>
    /// <remarks>
    /// This method orders the dictionary by frequency in descending order before printing.
    /// It outputs the results to the console with white text.
    /// </remarks>
    public void PrintLetterCount(Dictionary<char, int> characterCount)
    {
        // Order the dictionary by the frequency (value) in descending order
        foreach (var letter in characterCount.OrderByDescending(x => x.Value))
        {
            // Set the console text color to white
            Console.ForegroundColor = ConsoleColor.White;
            // Print the character and its frequency to the console
            Console.WriteLine($"{letter.Key}: {letter.Value}");
        }
    }
}