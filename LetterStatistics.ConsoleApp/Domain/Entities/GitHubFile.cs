using Newtonsoft.Json;

namespace LetterStatistics.ConsoleApp.Domain.Entities;

public class GitHubFile
{
    // The name of the file in the GitHub repository (e.g., "index.js", "style.css")
    [JsonProperty("name")]
    public string? Name { get; set; }

    // The URL where the file can be downloaded from GitHub
    [JsonProperty("download_url")]
    public string? Download_Url { get; set; }

    // The type of the file (e.g., "file" or "dir" for directories)
    [JsonProperty("type")]
    public string? Type { get; set; }

    // The path to the file within the repository, relative to the root
    [JsonProperty("path")]
    public string? Path { get; set; }
}