using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    public class CommonNinjectModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<ILogProvider>().To<LogProvider>().InSingletonScope();
            Bind<JobContext>().To<JobContext>().InSingletonScope();
        }
    }

}
