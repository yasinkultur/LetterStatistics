using LetterStatistics.ConsoleApp.Application.Interfaces;
using LetterStatistics.ConsoleApp.Domain.Services;
using LetterStatistics.ConsoleApp.Infrastructure.GitHub;
using LetterStatistics.ConsoleApp.Infrastructure.Services;

namespace LetterStatistics.ConsoleApp.Application.Services;


public class GitHubRepository : IGitHubRepository
{
    private readonly IGitHubApiClient _apiClient;
    private readonly CharacterCounterService _letterCounter;
    private readonly ILoggerService _logger;

    // Constructor to inject dependencies for the GitHubRepository
    // This constructor accepts the necessary services (API client, letter counter, and logger) via dependency injection
    public GitHubRepository(IGitHubApiClient apiClient, CharacterCounterService letterCounter, ILoggerService logger)
    {
        _apiClient = apiClient;  // Initialize the API client used to fetch file data
        _letterCounter = letterCounter;  // Initialize the service that counts letters in files
        _logger = logger;  // Initialize the logger to log information and errors
    }

    /// <summary>
    /// Processes files in the root directory of the GitHub repository, counting letters in each file.
    /// </summary>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method fetches the list of JavaScript/TypeScript files, reads each file's content,
    /// counts the frequency of each letter, and aggregates the results.
    /// </remarks>
    // This method processes files in the root directory by calling GetFileNamesAsync and GetFileContentAsync for each file.
    public async Task ProcessFilesAsync()
    {
        try
        {
            // Fetch the file names from the GitHub repository
            var fileNames = await _apiClient.GetFileNamesAsync();
            _logger.LogInformation($"Found {fileNames.Count} js/ts files.");

            var characterFrequency = new Dictionary<char, int>();  // Dictionary to store the character frequency across all files

            // Loop through each file name
            foreach (var fileName in fileNames)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;  // Set console color for output
                Console.WriteLine($"Reading file: {fileName}");

                // Fetch the content of each file
                var content = await _apiClient.GetFileContentAsync(fileName);
                // Count the letter frequencies in the file content
                var fileLetterCount = _letterCounter.CountLetters(content);

                // Aggregate the letter frequencies into the overall frequency dictionary
                foreach (var letter in fileLetterCount)
                {
                    if (characterFrequency.ContainsKey(letter.Key))
                    {
                        characterFrequency[letter.Key] += letter.Value;
                    }
                    else
                    {
                        characterFrequency[letter.Key] = letter.Value;
                    }
                }
            }

            // Print the aggregated letter frequencies
            _letterCounter.PrintLetterCount(characterFrequency);
        }
        catch (Exception ex)
        {
            // Log the error if any exception occurs during processing
            _logger.LogError($"An error occurred: {ex.Message}");
            throw;  // Re-throw the exception after logging
        }
    }

    /// <summary>
    /// Processes files recursively in the GitHub repository, counting letters in each file.
    /// </summary>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method fetches the list of JavaScript/TypeScript files recursively, reads each file's content,
    /// counts the frequency of each letter, and aggregates the results.
    /// </remarks>
    // This method processes files recursively by calling GetFileNamesAsyncRecursively and GetFileContentAsync for each file.
    public async Task ProcessFilesRecursivelyAsync()
    {
        try
        {
            // Fetch the file names recursively from the GitHub repository
            var fileNames = await _apiClient.GetFileNamesAsyncRecursively();
            _logger.LogInformation($"Found {fileNames.Count} js/ts files.");

            var characterFrequency = new Dictionary<char, int>();  // Dictionary to store the character frequency across all files

            // Loop through each file name
            foreach (var fileName in fileNames)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;  // Set console color for output
                Console.WriteLine($"Reading file: {fileName}");

                // Fetch the content of each file recursively
                var contentRecursively = await _apiClient.GetFileContentAsync(fileName);
                // Count the letter frequencies in the file content
                var fileLetterCountRecursively = _letterCounter.CountLetters(contentRecursively);

                // Aggregate the letter frequencies into the overall frequency dictionary
                foreach (var letter in fileLetterCountRecursively)
                {
                    if (characterFrequency.ContainsKey(letter.Key))
                    {
                        characterFrequency[letter.Key] += letter.Value;
                    }
                    else
                    {
                        characterFrequency[letter.Key] = letter.Value;
                    }
                }
            }

            // Print the aggregated letter frequencies
            _letterCounter.PrintLetterCount(characterFrequency);
        }
        catch (Exception ex)
        {
            // Log the error if any exception occurs during processing
            _logger.LogError($"An error occurred: {ex.Message}");
            throw;  // Re-throw the exception after logging
        }
    }
}