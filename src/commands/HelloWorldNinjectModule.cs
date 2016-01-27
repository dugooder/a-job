using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commands.helloworldcommand
{
    public class HelloWorldNinjectModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<common.ICommand>().To<HelloWorldCommand>().Named("HelloWorld");
        }
    }
}