using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Models;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace Agile.Minimalist.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly ISolrOperations<Hitter> _ctx;
        public CardRepository(ISolrOperations<Hitter> ctx)
        {
            _ctx = ctx; //ServiceLocator.Current.GetInstance<ISolrOperations<Quote>>();
        }
        public HitterSearchViewModel GetHitterSearch(HitterSearch criteria)
        {
            var query = new SolrQueryByRange<Int32>("year", criteria.YearStart, criteria.YearEnd, true)
                            && new SolrQueryByRange<Int32>("salary", 0, criteria.MaxSalary)
                            && new SolrQueryByRange<Int32>("homeruns", criteria.MinHomeRuns, 500);

            var options = GetFactedQueryOptions(criteria.MinHomeRuns);

            var results = _ctx.Query(query, options);

            var searchModel = new HitterSearchViewModel();
            SetSearchModelMainResults(results, searchModel);
            searchModel.OriginalCriteria = criteria;

            GenerateFacetedSearchCategories(results, searchModel);

            searchModel.NumberFound = results.NumFound;
            searchModel.RecordsShown = results.Count;

            return searchModel;
        }

        public HitterSearchViewModel GetFacetedHitterSearch(Int32? yearStart, Int32? yearEnd, Int32? maxSalary, Int32? minHomeRuns, string field, string criteria)
        {
            var query = new SolrQueryByRange<Int32>("year", yearStart.Value, yearEnd.Value, true)
                && new SolrQueryByRange<Int32>("salary", 0, maxSalary.Value)
                && new SolrQueryByRange<Int32>("homeruns", minHomeRuns.Value, 500)
                && new SolrQuery(String.Format("{0}:{1}", field, criteria));

            var options = GetFactedQueryOptions(minHomeRuns.Value);

            var results = _ctx.Query(query, options);
            var searchModel = new HitterSearchViewModel();
            SetSearchModelMainResults(results, searchModel);
            searchModel.OriginalCriteria = new HitterSearch()
            {
                MaxSalary = maxSalary.Value,
                MinHomeRuns = minHomeRuns.Value,
                YearEnd = yearEnd.Value,
                YearStart = yearStart.Value
            };

            GenerateFacetedSearchCategories(results, searchModel);

            searchModel.NumberFound = results.NumFound;
            searchModel.RecordsShown = results.Count;

            return searchModel;
        }


        private QueryOptions GetFactedQueryOptions(Int32 minHomeRuns)
        {
            var options = new QueryOptions()
            {
                OrderBy = new[] { new SortOrder("salary", Order.ASC), new SortOrder("homeruns", Order.DESC) },
                Rows = 50,
                Facet = new FacetParameters()
                {
                    Queries = FacetQueryCategories(minHomeRuns)
                }
            };
            return options;
        }

        private SolrFacetQuery[] FacetQueryCategories(Int32 minHomeRuns)
        {
            var salaryFacet1 = new SolrQueryByRange<Int32>("salary", 0, 1000000);
            var salaryFacet2 = new SolrQueryByRange<Int32>("salary", 1000001, 5000000);
            var salaryFacet3 = new SolrQueryByRange<Int32>("salary", 5000001, 10000000);
            var salaryFacet4 = new SolrQueryByRange<Int32>("salary", 10000001, 100000000);

            var yearFacet1 = new SolrQueryByRange<Int32>("year", 1950, 1959);
            var yearFacet2 = new SolrQueryByRange<Int32>("year", 1960, 1969);
            var yearFacet3 = new SolrQueryByRange<Int32>("year", 1970, 1979);
            var yearFacet4 = new SolrQueryByRange<Int32>("year", 1980, 1989);
            var yearFacet5 = new SolrQueryByRange<Int32>("year", 1990, 1999);
            var yearFacet6 = new SolrQueryByRange<Int32>("year", 2000, 2009);
            var yearFacet7 = new SolrQueryByRange<Int32>("year", 2010, 2013);

            var hrFacet1 = new SolrQueryByRange<Int32>("homeruns", minHomeRuns, minHomeRuns + 10);
            var hrFacet2 = new SolrQueryByRange<Int32>("homeruns", minHomeRuns + 11, minHomeRuns + 30);
            var hrFacet3 = new SolrQueryByRange<Int32>("homeruns", minHomeRuns + 31, minHomeRuns + 40);
            var hrFacet4 = new SolrQueryByRange<Int32>("homeruns", minHomeRuns + 41, minHomeRuns + 80);

            var doubleFacet1 = new SolrQueryByRange<Int32>("doubles", 0, 20);
            var doubleFacet2 = new SolrQueryByRange<Int32>("doubles", 21, 40);
            var doubleFacet3 = new SolrQueryByRange<Int32>("doubles", 41, 60);


            return new[] { 
                        new SolrFacetQuery(salaryFacet1),
                        new SolrFacetQuery(salaryFacet2),
                        new SolrFacetQuery(salaryFacet3),
                        new SolrFacetQuery(salaryFacet4),
                        new SolrFacetQuery(yearFacet1),
                        new SolrFacetQuery(yearFacet2),
                        new SolrFacetQuery(yearFacet3),
                        new SolrFacetQuery(yearFacet4),
                        new SolrFacetQuery(yearFacet5),
                        new SolrFacetQuery(yearFacet6),
                        new SolrFacetQuery(yearFacet7),
                        new SolrFacetQuery(hrFacet1),
                        new SolrFacetQuery(hrFacet2),
                        new SolrFacetQuery(hrFacet3),
                        new SolrFacetQuery(hrFacet4),
                        new SolrFacetQuery(doubleFacet1),
                        new SolrFacetQuery(doubleFacet2),
                        new SolrFacetQuery(doubleFacet3)
                    };
        }

        private static void SetSearchModelMainResults(SolrQueryResults<Hitter> results, HitterSearchViewModel searchModel)
        {
            searchModel.SearchResults = results.Select(r => new HitterSearchResult
            {
                Average = r.Average,
                Doubles = r.Doubles,
                FirstName = r.FirstName,
                Hits = r.Hits,
                HomeRuns = r.HomeRuns,
                Id = r.Id,
                LahmanId = r.LahmanId,
                LastName = r.LastName,
                PlayerId = r.PlayerId,
                RunsBattedIn = r.RunsBattedIn,
                Salary = r.Salary,
                StrikeOuts = r.StrikeOuts,
                TeamName = r.TeamName,
                Triples = r.Triples,
                Year = r.Year
            }).ToList();
        }

        private static void GenerateFacetedSearchCategories(SolrQueryResults<Hitter> results, HitterSearchViewModel searchModel)
        {
            searchModel.Categories = new Dictionary<String, HitterCategory>();
            foreach (var category in results.FacetQueries)
            {
                var categoryinfo = category.Key.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (searchModel.Categories.ContainsKey(categoryinfo[0]))
                {
                    //key exists, just add the item info
                    searchModel.Categories[categoryinfo[0]].Items.Add(new CategoryItem()
                    {
                        Count = category.Value,
                        Item = categoryinfo[1].TrimStart('[').TrimEnd(']'),
                        ItemCriteria = categoryinfo[1]
                    });
                }
                else
                {
                    //key doesn't exist...create it
                    searchModel.Categories.Add(categoryinfo[0], new HitterCategory()
                    {
                        CategoryField = categoryinfo[0],
                        Items = new List<CategoryItem>() { new CategoryItem() { 
                            Count = category.Value,
                            Item = categoryinfo[1].TrimStart('[').TrimEnd(']'),
                            ItemCriteria = categoryinfo[1]
                        }}
                    });
                }
            }
        }

    }
}
