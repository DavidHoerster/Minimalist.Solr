using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Minimalist.Commands.Baseball
{
    public class AddPlayerYear : CommandBase
    {
        public Int32 LahmanId { get; set; }
        public Int32 Year { get; set; }

        public Int32 Hits { get; set; }
        public Int32 HomeRuns { get; set; }
        public Double Average { get; set; }
        public Int32 RunsBattedIn { get; set; }
        public Int32 StrikeOuts { get; set; }
        public Int32 Doubles { get; set; }
        public Int32 Triples { get; set; }
        public Int32 Salary { get; set; }
        public String TeamName { get; set; }
    }
}
