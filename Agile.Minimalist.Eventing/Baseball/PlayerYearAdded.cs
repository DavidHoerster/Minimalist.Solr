using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Minimalist.Eventing.Baseball
{
    public class PlayerYearAdded : EventBase
    {
        public Int32 Id { get; set; }
        public Int32 Year { get; set; }
        public String PlayerId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Int32 Hits { get; set; }
        public Int32 HomeRuns { get; set; }
        public Double Average { get; set; }
        public Int32 RunsBattedIn { get; set; }
        public Int32 StrikeOuts { get; set; }
        public Int32 Doubles { get; set; }
        public Int32 Triples { get; set; }
        public Int32 Salary { get; set; }
        public String Team { get; set; }
    }
}
