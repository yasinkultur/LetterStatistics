using LetterStatistics.ConsoleApp.Application.Services;
using LetterStatistics.ConsoleApp.Domain.Services;
using LetterStatistics.ConsoleApp.Infrastructure.GitHub;
using LetterStatistics.ConsoleApp.Infrastructure.Services;
using Moq;

namespace LetterStatistics.Tests.ApplicationTests;

public class GitHubRepositoryTests
{
    // Test case to handle a GitHub API error correctly
    [Fact]
    public async Task ProcessFilesAsync_HandlesGitHubApiError_Successfully()
    {
        // Arrange: Mock the dependencies
        var mockApiClient = new Mock<IGitHubApiClient>();  // Mock the GitHub API client
        var mockLetterCounter = new Mock<CharacterCounterService>(); // Mock the letter counting service
        var mockLogger = new Mock<ILoggerService>();  // Mock the logging service

        // Setup the mock to throw an exception when GetFileNamesAsync is called
        mockApiClient.Setup(x => x.GetFileNamesAsync()).ThrowsAsync(new Exception("GitHub API Error"));

        // Create the repository using the mocked dependencies
        var repository = new GitHubRepository(mockApiClient.Object, mockLetterCounter.Object, mockLogger.Object);

        // Act & Assert: Verify that calling ProcessFilesAsync throws the expected exception
        await Assert.ThrowsAsync<Exception>(async () => await repository.ProcessFilesAsync());

        // Verify: Ensure that the error was logged
        mockLogger.Verify(logger => logger.LogError(It.Is<string>(s => s.Contains("An error occurred", StringComparison.OrdinalIgnoreCase))), Times.Once);
    }

    // Test case to verify that processing an empty file works correctly
    [Fact]
    public async Task ProcessFilesAsync_HandlesEmptyFile()
    {
        // Arrange: Mock the dependencies
        var mockApiClient = new Mock<IGitHubApiClient>();
        var mockLetterCounter = new Mock<CharacterCounterService>();
        var mockLogger = new Mock<ILoggerService>();

        // Setup the mock to return a list with a single "empty" file
        mockApiClient.Setup(x => x.GetFileNamesAsync()).ReturnsAsync(new List<string> { "empty-file.js" });
        mockApiClient.Setup(x => x.GetFileContentAsync(It.IsAny<string>())).ReturnsAsync("");  // Empty content

        // Create the repository using the mocked dependencies
        var repository = new GitHubRepository(mockApiClient.Object, mockLetterCounter.Object, mockLogger.Object);

        // Act: Call ProcessFilesAsync to process the empty file
        await repository.ProcessFilesAsync();

        // Assert: Verify that the log contains the message for processing 1 file
        mockLogger.Verify(logger => logger.LogInformation(It.Is<string>(s => s.Contains("Found 1 js/ts files", StringComparison.OrdinalIgnoreCase))), Times.Once);
    }

    // Test case to verify that a file with no letters is handled correctly
    [Fact]
    public async Task ProcessFilesAsync_HandlesFileWithNoLetters()
    {
        // Arrange: Mock the dependencies
        var mockApiClient = new Mock<IGitHubApiClient>();
        var mockLetterCounter = new Mock<CharacterCounterService>();
        var mockLogger = new Mock<ILoggerService>();

        // Setup the mock to return a file with no letters (only numbers)
        mockApiClient.Setup(x => x.GetFileNamesAsync()).ReturnsAsync(new List<string> { "fileWithNoLetters.js" });
        mockApiClient.Setup(x => x.GetFileContentAsync(It.IsAny<string>())).ReturnsAsync("1234567890");  // No letters

        // Create the repository using the mocked dependencies
        var repository = new GitHubRepository(mockApiClient.Object, mockLetterCounter.Object, mockLogger.Object);

        // Act: Call ProcessFilesAsync to process the file with no letters
        await repository.ProcessFilesAsync();

        // Assert: Verify that the log contains the message for processing 1 file with no letters
        mockLogger.Verify(logger => logger.LogInformation(It.Is<string>(s => s.Contains("Found 1 js/ts files."))), Times.Once);
    }

    // Integration test to verify the interaction between the repository and the API client
    [Fact]
    public async Task ProcessFilesAsync_IntegrationTest()
    {
        // Arrange: Mock the dependencies
        var mockApiClient = new Mock<IGitHubApiClient>();
        var mockLetterCounter = new Mock<CharacterCounterService>();
        var mockLogger = new Mock<ILoggerService>();

        // Setup the mock to return a list of two files
        var fileList = new List<string> { "x1.js", "x2.ts" };
        mockApiClient.Setup(x => x.GetFileNamesAsync()).ReturnsAsync(fileList);

        // Setup mock to return specific file content for each file
        mockApiClient.Setup(x => x.GetFileContentAsync(It.Is<string>(s => s == "x1.js"))).ReturnsAsync("const a = 1;");
        mockApiClient.Setup(x => x.GetFileContentAsync(It.Is<string>(s => s == "x2.ts"))).ReturnsAsync("let b = 2;");

        // Create the repository using the mocked dependencies
        var repository = new GitHubRepository(mockApiClient.Object, mockLetterCounter.Object, mockLogger.Object);

        // Act: Call ProcessFilesAsync to process the files
        await repository.ProcessFilesAsync();

        // Assert: Verify that the mock methods were called the expected number of times
        mockApiClient.Verify(client => client.GetFileNamesAsync(), Times.Once);  // Should be called once to get file names
        mockApiClient.Verify(client => client.GetFileContentAsync(It.IsAny<string>()), Times.Exactly(2));  // Should be called twice for the two files

        // Verify: Ensure that the log contains the expected message about the files processed
        mockLogger.Verify(logger => logger.LogInformation(It.Is<string>(s => s.Contains("Found 2 js/ts files"))), Times.Once);
    }
}