using common;
using System;
using Xunit;
using Xunit.Abstractions;

namespace tests
{
    public class JobContextTests : BaseTest
    {
        public JobContextTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper) { }

        [Fact()]
        public void UseJobContextTest()
        {
            DateTime myDob = new DateTime(2001, 04, 01);
            FakeJobContextClass classStoredInContext = new FakeJobContextClass();

            JobContext ctx = new JobContext(this.FakeLogger);
            ctx.Data.Add("name", "bill");
            ctx.Data.Add("dob", myDob);
            ctx.Data.Add("aclass", classStoredInContext);

            Assert.Equal("bill", ctx.Data["name"].ToString());
            Assert.Equal(myDob, (DateTime) ctx.Data["dob"]);
            Assert.Equal(false, ((FakeJobContextClass) ctx.Data["aclass"]).Disposed);

            ctx.Dispose();

            Assert.True(classStoredInContext.Disposed);
        }
    }
}