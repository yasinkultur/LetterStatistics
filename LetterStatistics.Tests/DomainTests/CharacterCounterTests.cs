using System.Diagnostics;
using LetterStatistics.ConsoleApp.Domain.Services;

namespace LetterStatistics.Tests.DomainTests;

public class CharacterCounterTests
{
    // Test to verify that CountLetters correctly counts the frequency of characters in a string
    [Fact]
    public void CountLetters_ReturnsCorrectFrequencyTotal_ForValidContent()
    {
        // Arrange: Initialize the CharacterCounterService instance and a sample content string
        var characterCounter = new CharacterCounterService();
        var content = "test word";  // The content to count letter frequencies

        // Act: Call the CountLetters method to get the frequency of each character
        var frequency = characterCounter.CountLetters(content);

        // Assert: Verify that the frequency counts are correct for each character
        Assert.Equal(2, frequency['t']);  // 't' appears 2 times
        Assert.Equal(1, frequency['e']);  // 'e' appears 1 time
        Assert.Equal(1, frequency['s']);  // 's' appears 1 time
        Assert.Equal(1, frequency['o']);  // 'o' appears 1 time
    }

    // Test to verify that CountLetters returns an empty dictionary for an empty string
    [Fact]
    public void CountLetters_ReturnsEmpty_ForEmptyContent()
    {
        // Arrange: Initialize the CharacterCounterService instance
        var characterCounter = new CharacterCounterService();

        // Act: Call the CountLetters method with an empty string
        var frequency = characterCounter.CountLetters(string.Empty);

        // Assert: Verify that the result is an empty dictionary since there are no characters to count
        Assert.Empty(frequency);  // The frequency dictionary should be empty for an empty input string
    }

    // Test to ensure CountLetters handles strings with no letter characters
    [Fact]
    public void CountLetters_HandlesNoLetterCharacters()
    {
        // Arrange: Initialize the CharacterCounterService instance and a string with non-letter characters
        var characterCounter = new CharacterCounterService();
        var content = "1234!@#$%^&*()";  // Non-letter characters only

        // Act: Call the CountLetters method to get the frequency of characters
        var frequency = characterCounter.CountLetters(content);

        // Assert: Verify that the result is an empty dictionary since there are no letters in the content
        Assert.Empty(frequency);  // No letters should be counted in the string containing only numbers and symbols
    }

    // Performance test to ensure CountLetters can handle a large input efficiently
    [Fact]
    public void CountLetters_PerformanceTest_LargeInput()
    {
        // Arrange: Initialize the CharacterCounterService instance and create a large input string
        var characterCounter = new CharacterCounterService();
        var largeInput = new string('a', 10_000_000);  // Create a string with 10 million 'a' characters

        // Start a stopwatch to measure performance
        var stopwatch = Stopwatch.StartNew();

        // Act: Call the CountLetters method to count the frequency of 'a' characters
        var frequency = characterCounter.CountLetters(largeInput);

        // Stop the stopwatch to measure elapsed time
        stopwatch.Stop();

        // Assert: Verify that 'a' is counted 10 million times and that the processing time is under 1000ms
        Assert.Equal(10_000_000, frequency['a']);  // Verify that the frequency of 'a' is correct
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, "Performance test failed: it took too long to process.");  // Assert that processing time is under 1 second
    }
}