using Ninject;
using Xunit.Abstractions;

namespace tests
{
    public abstract class BaseTest
    {
        protected FakeLogProvider FakeLogger;
        protected ITestOutputHelper TestOutputHelper;
        protected IKernel Kernel;

        protected BaseTest(ITestOutputHelper output)
        {
            this.TestOutputHelper = output;

            FakeLogger = new FakeLogProvider(output);

            StandardKernel stdKernel = new Ninject.StandardKernel();

            // prefixed all the DLLS with "ajob" because it was trying to load 
            // microsoft DLLS and getting a file not found
            stdKernel.Load(common.Job.NinjectDllsToLoadMask);

            this.Kernel = stdKernel;
        }
    }
}