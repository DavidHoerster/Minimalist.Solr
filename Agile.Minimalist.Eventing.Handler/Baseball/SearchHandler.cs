using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Common;
using Agile.Minimalist.Eventing.Baseball;
using Agile.Minimalist.Models;

namespace Agile.Minimalist.Eventing.Handler.Baseball
{
    public class SearchHandler : IHandle<PlayerYearAdded>
    {
        public void Handle(PlayerYearAdded theEvent)
        {
            //add this to Solr
            var hitter = new Hitter()
            {
                Average = theEvent.Average,
                Doubles = theEvent.Doubles,
                FirstName = theEvent.FirstName,
                Hits = theEvent.Hits,
                HomeRuns = theEvent.HomeRuns,
                Id = String.Format("{0}_{1}", theEvent.Id.ToString(), theEvent.Year.ToString()),
                LahmanId = theEvent.Id,
                LastName = theEvent.LastName,
                PlayerId = theEvent.PlayerId,
                RunsBattedIn = theEvent.RunsBattedIn,
                Salary = theEvent.Salary,
                StrikeOuts = theEvent.StrikeOuts,
                TeamName = theEvent.Team,
                Triples = theEvent.Triples,
                Year = theEvent.Year
            };
            var search = SearchFactory.GetSearchService();
            search.AddHitter(hitter);
        }
    }
}
