using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common;

namespace tests
{
    class FakeSimpleCommand : BaseCommand
    {
        public FakeSimpleCommand(ILogProvider log, JobContext ctx) : base(log, ctx) { }

        protected override void ExecuteImplementation()
        {
            ctx.Data.Add("BestDogEver", "Oliver");
            
            this.Successful = true;

            this.Result = 0;

            log.WriteMessage("Complete");
        }

    }
}

