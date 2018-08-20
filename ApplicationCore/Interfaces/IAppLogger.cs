using System;

namespace ApplicationCore.Interfaces
{
    public interface IAppLogger<T>
    {
        string Name { get; set; }

        void Trace(string message, params object[] args);
        void Debug(string message, params object[] args);
        void Info(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Warn(Exception ex, string message, params object[] args);
        void Error(Exception ex, string message, params object[] args);
        void Fatal(Exception ex, string message, params object[] args);
    }
}
