using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Ninject;

namespace common
{
    public sealed class Job : ICommand
    {
        public const string JobConfigurationFileExtension = "_job.xml";
        public const string NinjectDllsToLoadMask = "ajob.*.dll";

        private readonly IKernel kernel; // used to load the commands needed for this job
        private readonly ILogProvider log;

        public List<JobStep> Steps { get; private set; }
        public string Name { get; set; }
        public int Result { get; private set; }
        public bool Successful { get; private set; }

        public Job(string jobName, XDocument jobConfig, IKernel kernel, ILogProvider log)
        {
            this.log = log;
            this.kernel = kernel;
            Name = jobName;
            loadSteps(jobConfig);
            Result = 0;
            Successful = true;
        }

        public void Execute()
        {
            Successful = true;
            Result = 0;
            try
            {
                log.PushContextInfo(Name);

                log.WithLogLevel(LogLevel.Information).WriteMessage("Starting {0}", Name);

                foreach (var step in Steps)
                {
                    using (var cmd = kernel.Get<ICommand>(step.TypeName) as BaseCommand)
                    {
                        cmd.JobStep = step;
                        cmd.Name = step.Name;
                        cmd.Execute();
                        if (!cmd.Successful)
                        {
                            Successful = false;
                            Result = cmd.Result;
                            break;
                        }
                    }
                }
            }
            finally
            {
                log.WithLogLevel(LogLevel.Information).WriteMessage("{0} ended.", Name);
                log.PopContextInfo();
            }
        }

        public static int Run(string jobName)
        {
            var result = 0;

            using (var kernel =
                new StandardKernel())
            {
                kernel.Load(NinjectDllsToLoadMask);

                Debug.Assert(kernel.GetModules().Any());

                var log = kernel.Get<ILogProvider>();

                try
                {
                    var jobConfig =
                        XDocument.Load(jobName + JobConfigurationFileExtension);

                    var aJob = new Job(jobName, jobConfig, kernel, log);

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
            }

            return result;
        }

        private void loadSteps(XDocument config)
        {
            Steps = (from x in config.Descendants("step")
                select new JobStep
                {
                    Name = x.Attribute("name").Value,
                    TypeName = x.Attribute("type").Value,
                    Configuration = x
                }).ToList();

            log.WithLogLevel(LogLevel.Information).WriteMessage("Job {0} configured", Name);
        }
    }
}