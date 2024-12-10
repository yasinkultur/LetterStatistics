using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterStatistics.ConsoleApp.Application.Interfaces;

public interface IGitHubRepository
{
    /// <summary>
    /// Asynchronously processes files in the root directory of the GitHub repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method retrieves a list of JavaScript/TypeScript files from the root directory of the repository,
    /// counts the frequency of each letter in those files, and prints the results.
    /// </remarks>
    Task ProcessFilesAsync();

    /// <summary>
    /// Asynchronously processes files recursively in the GitHub repository, including files in subdirectories.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method retrieves a list of JavaScript/TypeScript files from the repository, including all subdirectories,
    /// counts the frequency of each letter in those files, and prints the results.
    /// </remarks>
    Task ProcessFilesRecursivelyAsync();
}