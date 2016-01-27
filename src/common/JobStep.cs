using System.Xml.Linq;

namespace common
{
    /// <summary>
    ///  Job step information class
    /// </summary>
    public sealed class JobStep
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public XElement Configuration { get; set; }
    }
}