using System;

namespace tests
{
    public sealed class FakeJobContextClass : IDisposable
    {
        public FakeJobContextClass()
        {
            this.Disposed = false;
        }

        public bool Disposed { get; set; }

        public void Dispose()
        {
            this.Disposed = true;
        }
    }
}