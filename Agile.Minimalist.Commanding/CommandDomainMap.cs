using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Minimalist.Commanding
{
    [Serializable]
    public class CommandDomainMap
    {
        public string CommandName { get; set; }
        public string DomainClassName { get; set; }
        public string DomainClassMethodName { get; set; }
    }
}
