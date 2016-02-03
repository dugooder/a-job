using Ninject;
using common;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Should;
using System;

namespace tests
{
    [Collection("Common")]
    public class LogProviderTests : BaseTest
    {
        ILogProvider logger;

        public LogProviderTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            logger = Kernel.Get<ILogProvider>();
        }

        [Fact()]
        public void PropertiesTest()
        {
            logger.WithProperty("color", "red").WithProperty("size", "large");
            Dictionary<string, object> allProps = logger.Properties;

            Assert.Equal(2, allProps.Keys.Count);
            Assert.Equal(allProps["color"].ToString(), "red");
            Assert.Equal(allProps["size"].ToString(), "large");
        }

        [Fact()]
        public void ResetTest()
        {
            ILogProvider provider = logger.WithProperty("color", "red");

            provider.Reset();

            Assert.False(logger.Properties.ContainsKey("color"));
        }

        [Fact()]
        public void WithPropertyTest()
        {
            ILogProvider provider = logger.WithProperty("color", "red");

            Assert.NotNull(provider);

            Assert.Equal("red", logger.Properties["color"]);
        }

     [Fact()]
        public void PushPopContextInfoTest()
        {
            logger.HasContextInfo().ShouldBeFalse();
            logger.PushContextInfo("UnitTest");
            logger.WithLogLevel(LogLevel.Debug).WriteMessage("a log message");
            logger.HasContextInfo().ShouldBeTrue();
            logger.PopContextInfo().Equals("UnitTest");
            logger.HasContextInfo().ShouldBeFalse();
        }

        public void UsingContextInfoTest()
        {
            logger.HasContextInfo().ShouldBeFalse();
            using (IDisposable logCtx = logger.PushContextInfo("UnitTest a"))
            {
                logger.WithLogLevel(LogLevel.Debug).WriteMessage("a log message");
                logger.HasContextInfo().ShouldBeTrue();
            }
            logger.HasContextInfo().ShouldBeFalse();
            
            logger.PopContextInfo().ShouldBeEmpty();
        }
    }
}