using System;
using System.Collections.Generic;

namespace common
{
    public enum LogLevel
    {
        Unknown,
        Debug,
        Information,
        Warning,
        Error
    }

    public interface ILogProvider
    {
        Dictionary<string, object> Properties { get; }

        object GetPropertyValue(string name, object defaultValue);

        ILogProvider WithProperty(string name, object value);

        void Reset();

        // This is not a dispoable object in the sense that it must be called
        //  it only removed the info from the stack.  The return value 
        //  can be ignored.
        IDisposable PushContextInfo(string info);

        string PopContextInfo();

        bool HasContextInfo();

        void Write(string logName, LogLevel level, object message, Exception ex);
    }
}