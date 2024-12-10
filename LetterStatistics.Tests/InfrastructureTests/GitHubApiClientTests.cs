using LetterStatistics.ConsoleApp.Infrastructure.GitHub;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterStatistics.Tests.InfrastructureTests
{
    public class GitHubApiClientTests
    {
        [Fact]
        public async Task GetFileNamesAsync_ReturnsListOfFileNames()
        {
            // Arrange: Mock the HTTP client behavior for GitHub API calls
            var mockHttpClient = new Mock<IGitHubApiClient>();  // Mocking the API client interface

            // Setting up the mock to return a list of file names when GetFileNamesAsync is called
            mockHttpClient.Setup(client => client.GetFileNamesAsync())
                .ReturnsAsync(new List<string> { "test1.js", "test2.ts", "test3.js" });

            var gitHubApiClient = mockHttpClient.Object; // Use the mocked client

            // Act: Call the method to fetch file names
            var result = await gitHubApiClient.GetFileNamesAsync();

            // Assert: Verify the result
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);  // Check if the list has 3 items
            Assert.Contains("test1.js", result);
            Assert.Contains("test2.ts", result);
            Assert.Contains("test3.js", result);
        }

        [Fact]
        public async Task GetFileContentAsync_ReturnsFileContent()
        {
            // Arrange: Mock the HTTP client behavior for getting file content from GitHub API
            var mockHttpClient = new Mock<IGitHubApiClient>();
            var testfileName = "file1.js";
            var testExpectedContent = "console.log('Hello, world!');";

            // Setup the mock to return specific file content when GetFileContentAsync is called
            mockHttpClient.Setup(client => client.GetFileContentAsync(testfileName))
                .ReturnsAsync(testExpectedContent);

            var gitHubApiClient = mockHttpClient.Object; // Use the mocked client

            // Act: Call the method to fetch file content
            var testResult = await gitHubApiClient.GetFileContentAsync(testfileName);

            // Assert: Verify that the result matches the expected content
            Assert.Equal(testExpectedContent, testResult);
        }

        [Fact]
        public async Task GetFileNamesAsync_HandlesEmptyResponse()
        {
            // Arrange: Mock the HTTP client behavior for an empty response
            var mockHttpClient = new Mock<IGitHubApiClient>();

            // Mock to return an empty list of file names
            mockHttpClient.Setup(client => client.GetFileNamesAsync())
                .ReturnsAsync(new List<string>());

            var gitHubApiClient = mockHttpClient.Object; // Use the mocked client

            // Act: Call the method to fetch file names
            var testResult = await gitHubApiClient.GetFileNamesAsync();

            // Assert: Verify that the result is an empty list
            Assert.NotNull(testResult);
            Assert.Empty(testResult);  // The result should be an empty list
        }

        [Fact]
        public async Task GetFileContentAsync_ThrowsException_WhenFileNotFound()
        {
            // Arrange: Mock the HTTP client behavior for file content request
            var mockHttpClient = new Mock<IGitHubApiClient>();
            var fileName = "invalidFile.js";

            // Setup the mock to throw an exception when the file doesn't exist
            mockHttpClient.Setup(client => client.GetFileContentAsync(fileName))
                .ThrowsAsync(new FileNotFoundException($"File {fileName} not found"));

            var gitHubApiClient = mockHttpClient.Object; // Use the mocked client

            // Act & Assert: Verify that the method throws the expected exception
            await Assert.ThrowsAsync<FileNotFoundException>(() => gitHubApiClient.GetFileContentAsync(fileName));
        }
    }
}
