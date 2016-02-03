using System;
using Ninject;

namespace common
{
    public abstract class BaseCommand : ICommand, IDisposable
    {
        protected readonly ILogProvider Log;
        protected readonly JobContext Context;

        [Inject]
        protected BaseCommand(ILogProvider log, JobContext ctx)
        {
            this.Log = log;
            this.Context = ctx;
        }

        public string Name { get; set; }
        public int Result { get; protected set; }
        public bool Successful { get; protected set; }
        public JobStep JobStep { get; set; }

        public void Execute()
        {
            using (IDisposable logCtx = Log.PushContextInfo(this.Name))
            {
                ExecuteImplementation();

                if (!Successful)
                {
                    Log.WithLogLevel(LogLevel.Error)
                        .WithProperty("Failing_Command", this.Name)
                        .WithProperty("Result_Code", this.Result)
                        .WriteProperties();
                }
            }
        }

        protected abstract void ExecuteImplementation();

        #region IDisposable 

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (Context != null)
                    {
                        Context.Dispose();
                    }
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}