using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Models;
using Microsoft.Practices.ServiceLocation;
using SolrNet;

namespace Agile.Common
{
    public class SearchService : ISearchService
    {
        private readonly SolrNet.ISolrOperations<Hitter> _ctx;
        public SearchService()
        {
            _ctx = ServiceLocator.Current.GetInstance<ISolrOperations<Hitter>>();
        }

        public void AddHitter(Hitter hitter)
        {
            _ctx.Add(hitter);
            _ctx.Commit();
        }
    }
}
