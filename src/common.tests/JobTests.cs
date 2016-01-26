using common;
using Ninject;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;

namespace tests
{
    public class JobTests : BaseTest
    {
        Job testJob;

        public JobTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            StandardKernel kernel = new Ninject.StandardKernel();

            kernel.Bind<ICommand>().To<FakeSimpleCommand>().Named("FakeCmd");
            kernel.Bind<ILogProvider>().To<FakeLogProvider>().InSingletonScope()
                .WithConstructorArgument("testOutputHelper", outputHelper);
            kernel.Bind<ICommand>().To<FakeSimpleCommand>();

            XDocument jobConfig = System.Xml.Linq.XDocument.Parse(
               "<job><step name=\"Acommand\" type=\"FakeCmd\"/></job>");

            testJob = new Job("TestJob", jobConfig, kernel, this.FakeLogger);
        }

        [Fact()]
        public void CreationTest()
        {
            Assert.Equal("TestJob", testJob.Name);
            Assert.Equal(0, testJob.Result);
            Assert.Equal(true, testJob.Successful);
            Assert.Equal(1, testJob.Steps.Count);
            Assert.Equal("Acommand", testJob.Steps[0].Name);
            Assert.Equal("FakeCmd", testJob.Steps[0].TypeName);
        }

        [Fact()]
        public void ExecuteTest()
        {
            testJob.Execute();

            Assert.Equal(0, testJob.Result);
            Assert.Equal(true, testJob.Successful);
            Assert.False(FakeLogger.Errors);
            Assert.False(FakeLogger.Warnings);
        }
    }
}
