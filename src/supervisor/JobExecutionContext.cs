using common;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace supervisor
{
    sealed class JobExecutionContext
    {   
        Dictionary<string, object> Data_;

        [Inject]
        public JobExecutionContext(string jobName)
        {
            this.JobName = jobName;
        }

        public string JobName { get; private set; }

        public Dictionary<string, object> Data
        {
            get
            {
                if (Data_ == null)
                {
                    this.Data_ = new Dictionary<string, object>();
                }
                return Data_;
            }
        }

        //public ICommand Job { get
        //    {
        //        if (Job_ == null)
        //        {
        //            JobCommand cmd = new JobCommand(this.JobName);
        //            cmd.Name = this.JobName;
        //            cmd.RunContext = this;
        //            Job_ = cmd;
        //        }
        //        return Job_;
        //    }
        //}
    }
}
