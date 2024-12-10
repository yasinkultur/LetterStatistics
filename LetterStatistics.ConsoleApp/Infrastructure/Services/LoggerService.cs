using Microsoft.Extensions.Logging;

namespace LetterStatistics.ConsoleApp.Infrastructure.Services;

public class LoggerService : ILoggerService
{
    // Private field to store the injected logger instance
    private readonly ILogger<LoggerService> _logger;

    // Constructor to initialize the logger instance
    // This constructor accepts an ILogger<LoggerService> instance via dependency injection.
    // ToolTip: ILogger<T> is a built-in interface from Microsoft.Extensions.Logging, allowing logging functionality 
    // specific to the LoggerService class. It provides methods to log messages at different levels of severity.
    public LoggerService(ILogger<LoggerService> logger)
    {
        _logger = logger;  // Store the injected logger instance for later use
    }

    /// <summary>
    /// Logs an informational message that is useful for tracking application flow or general status.
    /// </summary>
    /// <param name="message">The message to be logged.</param>
    public void LogInformation(string message)
    {
        _logger.LogInformation(message);  // Call the LogInformation method of the injected logger
    }

    /// <summary>
    /// Logs an error message when something goes wrong in the application.
    /// </summary>
    /// <param name="message">The error message to be logged.</param>
    public void LogError(string message)
    {
        _logger.LogError(message);  // Call the LogError method of the injected logger
    }

    /// <summary>
    /// Logs a critical error message for severe issues that may compromise the application's functionality.
    /// </summary>
    /// <param name="message">The critical error message to be logged.</param>
    public void LogCritical(string message)
    {
        _logger.LogCritical(message);  // Call the LogCritical method of the injected logger
    }

    /// <summary>
    /// Logs a debug message, typically used during development or troubleshooting.
    /// </summary>
    /// <param name="message">The debug message to be logged.</param>
    public void LogDebug(string message)
    {
        _logger.LogDebug(message);  // Call the LogDebug method of the injected logger
    }

    /// <summary>
    /// Logs a warning message indicating a potential issue or something that needs attention but doesn't require immediate action.
    /// </summary>
    /// <param name="message">The warning message to be logged.</param>
    public void LogWarning(string message)
    {
        _logger.LogWarning(message);  // Call the LogWarning method of the injected logger
    }
}