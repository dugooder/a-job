using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common;
using Ninject;

namespace common
{
    public abstract class BaseCommand : ICommand, IDisposable
    {
        protected readonly ILogProvider log;
        protected readonly JobContext ctx;
        
        [Inject]
        public BaseCommand(ILogProvider log, JobContext ctx)
        {
            this.log = log;
            this.ctx = ctx;
        }
               
        public string Name { get; set; }
        public int Result { get; protected set; }
        public bool Successful { get; protected set; }
        public JobStep JobStep { get; set; }

        public void Execute()
        {
            try
            {
                log.PushContextInfo(this.Name);

                ExecuteImplementation();

                if (!Successful)
                {
                    log.WithLogLevel(LogLevel.Error)
                        .WithProperty("Failing_Command", this.Name)
                        .WithProperty("Result_Code", this.Result)
                        .WriteProperties();
                }
            }
            finally
            {
                log.PopContextInfo();
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
                    if (ctx != null)
                    {
                        ctx.Dispose();
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
