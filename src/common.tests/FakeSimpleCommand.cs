using common;

namespace tests
{
    class FakeSimpleCommand : BaseCommand
    {
        public FakeSimpleCommand(ILogProvider log, JobContext ctx) 
            : base(log, ctx) { }
        
        protected override void ExecuteImplementation()
        {
            Context.Data.Add("BestDogEver", "Oliver");

            this.Successful = true;

            this.Result = 0;

            Log.WriteMessage("Complete");
        }
    }
}