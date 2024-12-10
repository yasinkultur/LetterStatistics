namespace LetterStatistics.ConsoleApp.Infrastructure.Services;

public interface ILoggerService
{
    /// <summary>
    /// Logs an informational message that is useful for tracking application flow or general status.
    /// </summary>
    /// <param name="message">The message to be logged.</param>
    /// <remarks>
    /// This method is typically used to log routine information, such as the completion of a task or the start of a process.
    /// </remarks>
    void LogInformation(string message);

    /// <summary>
    /// Logs an error message when something goes wrong in the application.
    /// </summary>
    /// <param name="message">The error message to be logged.</param>
    /// <remarks>
    /// This method should be used for logging error events, such as exceptions or failed operations.
    /// </remarks>
    void LogError(string message);

    /// <summary>
    /// Logs a critical error message for severe issues that may compromise the application's functionality.
    /// </summary>
    /// <param name="message">The critical error message to be logged.</param>
    /// <remarks>
    /// This method is used for logging critical issues that could lead to application failure or require immediate attention.
    /// </remarks>
    void LogCritical(string message);

    /// <summary>
    /// Logs a debug message, typically used during development or troubleshooting.
    /// </summary>
    /// <param name="message">The debug message to be logged.</param>
    /// <remarks>
    /// This method is used to log detailed information about the application’s internal state, useful for debugging.
    /// Debug messages are often not logged in production environments unless specifically enabled.
    /// </remarks>
    void LogDebug(string message);

    /// <summary>
    /// Logs a warning message indicating a potential issue or something that needs attention but doesn't require immediate action.
    /// </summary>
    /// <param name="message">The warning message to be logged.</param>
    /// <remarks>
    /// This method is used to log events that are not critical but might indicate a future problem, such as performance warnings or deprecated features.
    /// </remarks>
    void LogWarning(string message);
}
