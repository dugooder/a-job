using System;
using System.Collections.Generic;
using Ninject;
namespace common
{
    /// <summary>
    ///  The object common to all the commands
    /// </summary>
    public sealed class JobContext : IDisposable
    {   
        Dictionary<string, object> Data_;
        readonly ILogProvider log;

        [Inject]
        public JobContext(ILogProvider logger)
        {
            this.log = logger;
        }

        public Dictionary<string, object> Data
        {
            get
            {
                if (Data_ == null)
                {
                    this.Data_ = new Dictionary<string, object>();
                }
                return Data_;
            }
        }

        #region IDisposable
        private bool disposed = false; 

        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (Data_ != null)
                    {
                        IEnumerator<KeyValuePair<string, object>> ie = Data_.GetEnumerator();
                        while (ie.MoveNext())
                        {
                            if (ie.Current.Value != null && ie.Current.Value is IDisposable)
                            {
                                try {
                                    (ie.Current.Value as IDisposable).Dispose();
                                }
                                catch (Exception ex)
                                {
                                    log.WithLogLevel(LogLevel.Warning).WriteGeneralException(ex);     
                                }
                            }
                        }

                        Data_ = null;
                    }

                    disposed = true;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
