using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Models;

namespace Agile.Minimalist.Repository
{
    public interface ICardRepository
    {
        HitterSearchViewModel GetHitterSearch(HitterSearch criteria);
        HitterSearchViewModel GetFacetedHitterSearch(Int32? yearStart, Int32? yearEnd, Int32? maxSalary, Int32? minHomeRuns, String field, String criteria);
    }
}
