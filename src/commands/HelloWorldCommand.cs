using common;
using Ninject;

namespace commands
{
    /// <summary>
    /// Hello World Command project is an example of a command project.
    /// 1. A command should be in its own project 
    /// 2. A command should have its own test project
    /// 3. A command needs to inhert form BaseCommand
    /// 4. A command project needs to include a ninject module class 
    ///    to load the command into the Ioc container.
    ///    
    /// A command project could be referenced by the supervisor or not depending on 
    /// how you want to build the application.  The bottome line is the DLL, its dependencies
    /// and ninject.xml file must be put in the EXE's bin folder. 
    /// </summary>

    class HelloWorldCommand : BaseCommand
    {
        public const string ContextKeyCounter = "HelloWorldCommandCount";

        [Inject]
        public HelloWorldCommand(ILogProvider log, JobContext ctx) 
            : base(log, ctx) { }

        protected override void ExecuteImplementation()
        {
            int cnt = getCurrentCount() + 1;

            sayHello(cnt);

            ctx.Data[ContextKeyCounter] = cnt;

            this.Successful = true;

            this.Result = 0;
        }

        private int getCurrentCount()
        {
            int cnt = 0;

            if (ctx.Data.ContainsKey(ContextKeyCounter))
            {
                cnt = (int)ctx.Data[ContextKeyCounter];
            }

            return cnt;

        }
        private void sayHello(int cnt)
        {
            string name = 
                JobStep.Configuration.Element("name").Value;

            log.WithLogLevel(LogLevel.Information)
                .WriteMessage("Hello {0} ({1})", name, cnt);
        }
    }
}
