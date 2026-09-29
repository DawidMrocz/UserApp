using Common.Attributes.DependencyInjection;
using log4net;
using log4net.Core;

namespace Common.Services.Logger
{
    [DependencyInjection]
    public class Logger : ILogger
    {
        private log4net.Core.ILogger _logger;
        public Logger() => _logger = LogManager.GetLogger(GetType()).Logger;

        public void Log(Type type, Level logType, string message, Exception ex)
        {
            _logger.Log(type, logType, message, ex);
        }
    }
}
