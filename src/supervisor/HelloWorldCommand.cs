using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common;
using Ninject;

namespace supervisor
{
    class HelloWorldCommand : BaseCommand
    {
        [Inject]
        public HelloWorldCommand(ILogProvider log) : base(log) { }

        protected override void ExecuteImplementation()
        {
            Console.WriteLine("Hello world");
            this.Successful = true;
            this.Result = 0; 
        }
    }
}
