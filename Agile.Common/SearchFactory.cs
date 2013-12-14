using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Models;
using SolrNet;

namespace Agile.Common
{
    public static class SearchFactory
    {
        private static ISearchService _service;
        static SearchFactory()
        {
        }

        public static void Init()
        {
            Startup.Init<Hitter>("http://localhost:8983/solr/cqrsball");
            _service = new SearchService();
        }

        public static ISearchService GetSearchService()
        {
            return _service;
        }
    }
}
