using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace supervisor
{
    class JobStep
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public XNode Configuraiton { get; set; }

        internal ICommand CreateCommand()
        {
            throw new NotImplementedException();
        }
    }
}
