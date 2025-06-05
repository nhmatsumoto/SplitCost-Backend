using Serilog;
using SplitCost.Application.Interfaces;

namespace SplitCost.Infrastructure.Logging;

public class LoggerManager : ILoggerManager
{
    public void LogInfo(string message)
    {
        Log.Information(message);
    }

    public void LogWarn(string message)
    {
        Log.Warning(message);
    }

    public void LogDebug(string message)
    {
        Log.Debug(message);
    }

    public void LogError(string message, Exception? ex = null)
    {
        if (ex != null)
            Log.Error(ex, message);
        else
            Log.Error(message);
    }
}