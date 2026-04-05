namespace Template.Api.Brokers.Logging;

/// <summary>
/// LoggingBroker interface.
/// </summary>
public interface ILoggingBroker
{
    void LogInformation(string message);
    void LogError(Exception exception);
    void LogCritical(Exception exception);
    void LogDebug(string message);
}