﻿using common;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace tests
{
    public class FakeLogProvider : ILogProvider
    {
        Dictionary<string, object> props;
        readonly ITestOutputHelper testOutput;

        public Stack ContextInfo { get; private set; }
        public LogEntry LastLogEntry { get; private set; }
        public int ExceptionCount { get; private set; }
        public bool Errors { get; private set; }
        public bool Warnings { get; private set; }

        public FakeLogProvider(ITestOutputHelper testOutputHelper)
        {
            this.testOutput = testOutputHelper;
            props = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Properties
        {
            get
            {
                return props;
            }
        }

        public object GetPropertyValue(string name, object defaultValue)
        {
            object result = props.ContainsKey(name) ? 
                props[name] :  defaultValue;
            
            testOutput.WriteLine(
                "GetPropertyValue(name='{0}', defaultValue='{1}') = result='{2}'",
                name, defaultValue, result);

            return result;
        }

        public string PopContextInfo()
        {
            string result = null;

            if (ContextInfo != null)
            {
                result = ContextInfo.Pop() as string;
            }

            testOutput.WriteLine("PopContextInfo()");

            return result;
        }

        public IDisposable PushContextInfo(string info)
        {
            if (ContextInfo == null)
            {
                ContextInfo = new Stack();
            }

            ContextInfo.Push(info);

            testOutput.WriteLine("PushContextInfo(info='{0};)", info);

            return new FakeLoggingAutoPopStackFrame();
        }

        public bool HasContextInfo()
        {
            return ContextInfo.Count > 0;
        }

        public void Reset()
        {
            ContextInfo = null;
            LastLogEntry = null;
            Errors = false;
            Warnings = false;
            props.Clear();
            testOutput.WriteLine("Reset()");
        }

        public ILogProvider WithProperty(string name, object value)
        {
            props[name] = value;
            testOutput.WriteLine("WithProperty(name='{0}', value='{1};)", name, value);
            return this;
        }

        public void Write(string logName, LogLevel level, object message, Exception ex)
        {
            LogEntry entry = new LogEntry();
            entry.LogName = logName;
            entry.LogLevel = level;
            entry.Message = message;
            entry.Exception = ex;
            entry.ContextInfo = this.ContextInfo != null ? this.ContextInfo.Clone() as Stack : new Stack();
            this.LastLogEntry = entry;

            if (ex != null)
            {
                ExceptionCount += 1;
            }
            if (LogLevel.Error == level)
            {
                this.Errors = true;
            }
            if (LogLevel.Warning == level)
            {
                this.Warnings = true;
            }

            testOutput.WriteLine("Write(logName='{0}', level='{1}', message='{2}', ex='{3}')",
                logName, level, message, ex);
        }

        public class LogEntry
        {
            public Stack ContextInfo;
            public string LogName;
            public LogLevel LogLevel;
            public object Message;
            public Exception Exception;
        }
    }

    public class FakeLoggingAutoPopStackFrame : IDisposable
    {
        public void Dispose()
        {
            // do nothing
        }
    }
}