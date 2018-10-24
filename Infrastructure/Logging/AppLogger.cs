using ApplicationCore.Interfaces;
using NLog;
using System;

namespace Infrastructure.Logging
{
    public class AppLogger<T> : IAppLogger<T>
    {
        private static Logger _logger = LogManager.GetLogger(typeof(T).FullName);
        private static string _loggerName = typeof(T).FullName;

        public string Name
        {
            get  {  return _loggerName;  }
            set
            {
                if (_loggerName != value)
                {
                    _loggerName = value;
                    _logger = LogManager.GetLogger(_loggerName);
                }
            }
        }

        public AppLogger()
        {
            Name = typeof(T).Name + "Logger";
        }

        public void Trace(string message, params object[] args) => _logger.Trace(message, args);
        public void Debug(string message, params object[] args) => _logger.Debug(message, args);
        public void Info(string message, params object[] args) => _logger.Info(message, args);
        public void Warn(string message, params object[] args) => _logger.Warn(message, args);
        public void Warn(Exception ex, string message, params object[] args) => _logger.Warn(ex, message, args);
        public void Error(Exception ex, string message, params object[] args) => _logger.Error(ex, message, args);
        public void Fatal(Exception ex, string message, params object[] args) => _logger.Fatal(ex, message, args);

    }
}
