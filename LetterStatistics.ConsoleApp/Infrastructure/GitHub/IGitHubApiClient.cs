namespace LetterStatistics.ConsoleApp.Infrastructure.GitHub;

public interface IGitHubApiClient
{
    /// <summary>
    /// Asynchronously retrieves the list of file names from the GitHub repository's root directory.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is a list of file names as strings.</returns>
    /// <remarks>
    /// This method fetches file names from the root directory of the GitHub repository and filters out non-relevant files.
    /// Typically, this is used to retrieve JavaScript or TypeScript files from the repository.
    /// </remarks>
    Task<List<string>> GetFileNamesAsync();

    /// <summary>
    /// Asynchronously retrieves the content of a specific file from the GitHub repository.
    /// </summary>
    /// <param name="fileUrl">The URL of the file to fetch from GitHub.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the content of the file as a string.</returns>
    /// <remarks>
    /// This method fetches the content of a file from the GitHub repository given its raw file URL.
    /// The URL should point to the raw content of the file within the GitHub repository.
    /// </remarks>
    Task<string> GetFileContentAsync(string fileUrl);

    /// <summary>
    /// Asynchronously retrieves the list of file names from the GitHub repository, searching recursively in subdirectories.
    /// </summary>
    /// <param name="directory">The directory path to search from. Defaults to the root directory.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is a list of file names as strings.</returns>
    /// <remarks>
    /// This method fetches the file names from the GitHub repository, including files in subdirectories.
    /// If no directory is provided, it will search the entire repository recursively.
    /// </remarks>
    Task<List<string>> GetFileNamesAsyncRecursively(string directory = "");
}
