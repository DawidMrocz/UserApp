using log4net.Core;

namespace Common.Services.Logger
{
    public interface ILogger
    {
        void Log(Type type, Level logType, string message, Exception ex);
    }
}
