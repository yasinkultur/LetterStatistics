using LetterStatistics.ConsoleApp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace LetterStatistics.ConsoleApp.Infrastructure.GitHub;

public class GitHubApiClient : IGitHubApiClient
{
    private readonly IRestClient _client;
    private const string RepoOwner = "lodash"; // The owner of the GitHub repository
    private const string RepoName = "lodash";  // The name of the GitHub repository
    private const string ApiUrl = "https://api.github.com";  // The base URL for the GitHub API

    // Constructor accepts IConfigurationRoot to configure the API client with access token for GitHub authentication
    // Constructor that sets up the RestClient with default headers and GitHub API authentication
    public GitHubApiClient(IConfigurationRoot configuration)
    {
        _client = new RestClient(ApiUrl);
        _client.AddDefaultHeader("User-Agent", "LetterStatisticsApp");
        _client.AddDefaultHeader("Authorization", $"Bearer {configuration.GetValue<string>("AccessToken")}");
    }

    /// <summary>
    /// Retrieves the list of JavaScript/TypeScript file download URLs from the GitHub repository.
    /// </summary>
    /// <returns>A list of file download URLs for JS and TS files.</returns>
    /// <exception cref="Exception">Thrown when there is an error getting the file list from GitHub.</exception>
    // This method retrieves the file names from the root directory of the GitHub repository.
    public virtual async Task<List<string>> GetFileNamesAsync()
    {
        try
        {
            // Prepare the API request to get the contents of the repository
            var request = new RestRequest($"/repos/{RepoOwner}/{RepoName}/contents", Method.Get);
            var response = await _client.ExecuteAsync<List<GitHubFile>>(request);

            if (response.IsSuccessful)
            {
                // Filter and return the file URLs for .js and .ts files only
                return (from file in response.Data
                        where !string.IsNullOrEmpty(file.Name) && (file.Name.EndsWith(".js", StringComparison.OrdinalIgnoreCase) || file.Name.EndsWith(".ts", StringComparison.OrdinalIgnoreCase))
                        select file.Download_Url).ToList();
            }

            // Throw an exception if the request is not successful
            throw new Exception("Error getting file list from GitHub.");
        }
        catch (Exception ex)
        {
            // Catch and throw an exception with a detailed error message
            throw new Exception($"Error in GetFileNamesAsync: {ex.Message}");
        }
    }

    /// <summary>
    /// Recursively retrieves the list of JavaScript/TypeScript file download URLs from the GitHub repository, including subdirectories.
    /// </summary>
    /// <param name="directory">The directory path to start the search from (default is the root directory).</param>
    /// <returns>A list of file download URLs for JS and TS files, including recursively from subdirectories.</returns>
    /// <exception cref="Exception">Thrown when there is an error retrieving file contents from GitHub.</exception>
    // This method recursively searches directories for JS and TS files.
    public virtual async Task<List<string>> GetFileNamesAsyncRecursively(string directory = "")
    {
        try
        {
            var fileNames = new List<string>();  // List to store the file URLs

            // Prepare the API request to get the contents of the specified directory
            var request = new RestRequest($"/repos/{RepoOwner}/{RepoName}/contents/{directory}", Method.Get);
            var response = await _client.ExecuteAsync<List<GitHubFile>>(request);

            // Throw an exception if the request is not successful
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error getting contents from GitHub: {response.ErrorMessage}");
            }

            // Return an empty list if no files or directories are found
            if (response.Data == null || !response.Data.Any()) return fileNames;

            // Iterate through the files and directories
            foreach (var item in response.Data)
            {
                switch (item.Type)
                {
                    // If it's a JavaScript or TypeScript file, add its download URL to the list
                    case "file" when !string.IsNullOrEmpty(item.Name) && (item.Name.EndsWith(".js", StringComparison.OrdinalIgnoreCase) || item.Name.EndsWith(".ts", StringComparison.OrdinalIgnoreCase)):
                        if (!string.IsNullOrEmpty(item.Download_Url))
                        {
                            fileNames.Add(item.Download_Url);
                        }
                        break;

                    // If it's a directory, recursively fetch files from the subdirectory
                    case "dir":
                        if (!string.IsNullOrEmpty(item.Path))
                        {
                            var subDirectoryFiles = await GetFileNamesAsyncRecursively(item.Path);
                            fileNames.AddRange(subDirectoryFiles);
                        }
                        break;
                }
            }

            return fileNames;
        }
        catch (Exception ex)
        {
            // Catch and throw an exception with a detailed error message
            throw new Exception($"Error in GetFileNamesAsyncRecursively: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves the content of a specific file from the GitHub repository.
    /// </summary>
    /// <param name="fileUrl">The URL of the file to fetch.</param>
    /// <returns>The content of the file as a string.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file is not found at the provided URL.</exception>
    /// <exception cref="Exception">Thrown when there is an error retrieving the file content from GitHub.</exception>
    // This method retrieves file content from the GitHub repository.
    public virtual async Task<string> GetFileContentAsync(string fileUrl)
    {
        try
        {
            // Prepare the API request to get the file content
            var request = new RestRequest(fileUrl, Method.Get);
            var response = await _client.ExecuteAsync(request);

            // If the request is successful, return the file content
            if (response.IsSuccessful)
            {
                return response.Content ?? string.Empty;  // Return content or an empty string if the content is null
            }

            // Throw an exception if the file is not found
            throw new FileNotFoundException($"File {fileUrl} not found");
        }
        catch (Exception ex)
        {
            // Catch and throw an exception with a detailed error message
            throw new Exception($"Error in GetFileContentAsync: {ex.Message} {fileUrl}");
        }
    }
}