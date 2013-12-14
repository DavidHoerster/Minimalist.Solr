using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Minimalist.Models
{
    public class HitterFacetSearch
    {
        public Int32 YearStart { get; set; }
        public Int32 YearEnd { get; set; }
        public Int32 MinHomeRuns { get; set; }
        public Int32 MaxSalary { get; set; }
        public String field { get; set; }
        public String criteria { get; set; }
    }
}
