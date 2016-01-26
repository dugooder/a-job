using System;
using Ninject;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


namespace common
{
    public sealed class Job : ICommand
    {
        const string JobConfigurationFileExtension = "_job.xml";
        
        readonly ILogProvider log;
        readonly IKernel kernel;  // used to load the commands needed for this job

        public List<JobStep> Steps { get; private set; }
        public string Name { get; set; }
        public int Result { get; private set; }
        public bool Successful { get; private set; }

        public Job(string jobName, XDocument jobConfig, IKernel kernel, ILogProvider log)
        {
            this.log = log;
            this.kernel = kernel;
            this.Name = jobName;
            loadSteps(jobConfig);
            this.Result = 0;
            this.Successful = true;
        }
        
        public void Execute()
        {
            this.Successful = true;
            this.Result = 0;
            try
            {
                log.PushContextInfo(this.Name);

                log.WithLogLevel(LogLevel.Information).WriteMessage("Starting {0}", this.Name);

                foreach (JobStep step in this.Steps)
                {
                    using (BaseCommand cmd = kernel.Get<ICommand>(step.TypeName) as BaseCommand)
                    {
                        cmd.JobStep = step;
                        cmd.Name = step.Name;
                        cmd.Execute();
                        if (!cmd.Successful)
                        {
                            this.Successful = false;
                            this.Result = cmd.Result;
                            break;
                        }
                    }
                }
            }
            finally
            {
                log.WithLogLevel(LogLevel.Information).WriteMessage("{0} ended.", this.Name);
                log.PopContextInfo();
            }
        }

        public static int Run(string jobName)
        {
            int result = 0;
            
            using (Ninject.StandardKernel kernel = 
                new Ninject.StandardKernel( ))
            {
                kernel.Load("*.dll");  

                System.Diagnostics.Debug.Assert(kernel.GetModules().Count() > 0);
                
                ILogProvider log = kernel.Get<ILogProvider>();

                try
                {
                    XDocument jobConfig =
                        XDocument.Load(jobName + JobConfigurationFileExtension);

                    Job aJob = new Job(jobName, jobConfig, kernel, log);

                    aJob.Execute();

                    result = aJob.Result;

                    if (!aJob.Successful)
                    {
                        log.WithLogLevel(LogLevel.Error)
                            .WriteMessage("{0} failed.", aJob.Name);
                    }
                }
                catch (Exception ex)
                {
                    log.WithLogLevel(LogLevel.Error)
                        .WriteGeneralException(ex);

                    result = -1;
                }

                return result;
            }
        }

        private void loadSteps(XDocument config)
        {
            this.Steps = (from x in config.Descendants("step")
                          select new JobStep()
                          {
                              Name = x.Attribute("name").Value,
                              TypeName = x.Attribute("type").Value,
                              Configuration = x
                          }).ToList();

            log.WithLogLevel(LogLevel.Information).WriteMessage("Job {0} configured", this.Name);
        }
    }
}

