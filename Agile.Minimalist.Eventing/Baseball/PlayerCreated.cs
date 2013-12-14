using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Minimalist.Eventing.Baseball
{
    public class PlayerCreated : EventBase
    {
        public String FullName { get; set; }
        public String PlayerId { get; set; }
    }
}
