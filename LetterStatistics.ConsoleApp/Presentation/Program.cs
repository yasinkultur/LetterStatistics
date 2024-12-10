// Program.cs

using LetterStatistics.ConsoleApp.Application.Interfaces;
using LetterStatistics.ConsoleApp.Application.Services;
using LetterStatistics.ConsoleApp.Domain.Services;
using LetterStatistics.ConsoleApp.Infrastructure.GitHub;
using LetterStatistics.ConsoleApp.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace LetterStatistics.ConsoleApp.Presentation;


class Program
{
    // Configuration property to hold application settings
    public static IConfigurationRoot? Configuration;

    // The main entry point for the application
    static async Task Main(string[] args)
    {
        // Configure services and return the service provider
        var serviceProvider = ConfigureServices();

        // Get the logger service from the DI container
        var logger = serviceProvider.GetRequiredService<ILoggerService>();

        // Log that the program has started
        logger.LogInformation("Program started.");

        // If configuration is loaded, retrieve and log the access key
        if (Configuration != null)
        {
            var accessKey = Configuration.GetValue<string>("AccessToken");
            logger.LogInformation($"Access Key: {accessKey}");
        }

        // Get the GitHub repository service from the DI container
        var gitHubRepository = serviceProvider.GetRequiredService<IGitHubRepository>();

        string? input;
        do
        {
            // Set the console color to yellow for the prompt
            Console.ForegroundColor = ConsoleColor.Yellow;
            // Prompt user for input to choose an option
            Console.WriteLine("Enter '1' to search files in the root directory, or '2' to search all files recursively.");

            // Read user input from the console
            input = Console.ReadLine();

            // Handle user input with a switch statement
            switch (input)
            {
                case "1":
                    // Process files in the root directory
                    await gitHubRepository.ProcessFilesAsync();
                    break;

                case "2":
                    // Process files recursively in all directories
                    await gitHubRepository.ProcessFilesRecursivelyAsync();
                    break;

                default:
                    // Handle invalid input
                    Console.WriteLine("Invalid input. Please enter '1' or '2'.");
                    break;
            }
        }
        while (input != "1" && input != "2");  // Continue until valid input is entered

        Console.ReadLine();
    }

    // Method to configure services and return the service provider
    private static IServiceProvider ConfigureServices()
    {
        // Set up dependency injection container
        var serviceProvider = new ServiceCollection()
            .AddLogging(builder => builder.AddConsole()) // Add logging service with console output
            .AddSingleton<IGitHubApiClient, GitHubApiClient>()  // Register IGitHubApiClient with GitHubApiClient implementation
            .AddSingleton<CharacterCounterService>()  // Register character counter service
            .AddSingleton<ILoggerService, LoggerService>()  // Register logging service
            .AddSingleton<IGitHubRepository, GitHubRepository>();  // Register GitHub repository service

        // Set up configuration to read from appsettings.json
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)!.FullName) // Set base path for configuration
            .AddJsonFile("appsettings.json", false)  // Add appsettings.json file to configuration (no required reload)
            .Build();

        // Add the configuration to the DI container
        serviceProvider.AddSingleton(Configuration);

        // Build and return the service provider with all the services registered
        return serviceProvider.BuildServiceProvider();
    }
}