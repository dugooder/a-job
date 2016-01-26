using common;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace supervisor
{
    class JobCommand : BaseCommand
    {
        readonly IKernel kernel;
        public List<JobStep> Steps { get; private set; }

        [Inject]
        public JobCommand(string jobName, IKernel kernel, ILogProvider log)  : base(log)
        {
            this.kernel = kernel;
            XDocument xdoc = XDocument.Load(jobName + ".xml");
            this.Steps = (from x in xdoc.Descendants("step")
                          select new JobStep()
                          {
                              Name = x.Attribute("name").Value,
                              TypeName = x.Attribute("type").Value,
                              Configuraiton = x.DescendantNodes().FirstOrDefault()
                          }).ToList();

        }

        protected override void ExecuteImplementation()
        {

            foreach (JobStep step in this.Steps)
            {
                ICommand cmd = kernel.Get<ICommand>(step.TypeName);
                cmd.Name = step.Name;
                cmd.Context = this.RunContext;
                cmd.Execute();
                if (!cmd.Successful)
                {
                    this.Successful = false;
                    this.Result = cmd.Result;
                    break;
                }
            }
            Console.WriteLine("do something");
        }
    }
}
