using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Model;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace Agile.Minimalist.Repository
{
    public class QuoteRepository
    {
        private readonly ISolrOperations<Quote> _ctx;
        public QuoteRepository()
        {
            _ctx = ServiceLocator.Current.GetInstance<ISolrOperations<Quote>>();
        }

        public QuoteRepository(ISolrOperations<Quote> context)
        {
            _ctx = context;
        }

        public IEnumerable<Quote> GetAll()
        {
            var quotes = _ctx.Query(SolrQuery.All);
            var trimmedQuotes = quotes.Select(q => new Quote
            {
                Abstract = q.Abstract,
                ArticleBody = q.ArticleBody.Length < 200 ? q.ArticleBody : q.ArticleBody.Substring(0, 200),
                Id = q.Id,
                Source = q.Source,
                Title = q.Title,
                Year = q.Year
            });
            return trimmedQuotes;
        }

        public Quote GetById(String quoteId)
        {
            var quotes = _ctx.Query(new SolrQuery("id:" + quoteId));
            return quotes.FirstOrDefault();
        }

        public QuoteDetail GetWithDetail(String quoteId)
        {
            var theQuote = new QuoteDetail();
            var options = new QueryOptions()
            {
                MoreLikeThis = new MoreLikeThisParameters(new[] { "articlebody", "source" })
                {
                    MinDocFreq = 1,
                    MinTermFreq = 1
                }
            };
            var quotes = _ctx.Query(new SolrQuery("id:" + quoteId), options);

            if (quotes.Count > 0)
            {
                var similarQuotes = new List<QuoteDetail>();
                var quote = quotes[0];
                var similar = quotes.SimilarResults;
                foreach (var item in quotes.SimilarResults)
                {
                    foreach (var itemValue in item.Value)
                    {
                        var similarQuote = new QuoteDetail()
                        {
                            Id = itemValue.Id,
                            Title = itemValue.Title
                        };
                        similarQuotes.Add(similarQuote);
                    }
                }

                theQuote = new QuoteDetail()
                {
                    SimilarItems = similarQuotes,
                    Abstract = quote.Abstract,
                    ArticleBody = quote.ArticleBody,
                    Id = quote.Id,
                    Source = quote.Source,
                    Title = quote.Title,
                    Year = quote.Year
                };
            }
            return theQuote;
        }

        public Boolean Insert(Quote theQuote)
        {
            try
            {
                _ctx.Add(theQuote);
                _ctx.Commit();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean Update(Quote theQuote)
        {
            try
            {
                _ctx.Add(theQuote);
                _ctx.Commit();
                _ctx.Optimize();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void DeleteById(String quoteId)
        {
            _ctx.Delete(new SolrQuery("id:" + quoteId));
            _ctx.Commit();
        }

        public IEnumerable<QuoteHighlight> Search(String searchKey, out Int32 totalResults)
        {
            var query = new SolrQuery("text:" + searchKey);

            var options = new QueryOptions()
            {
                //I don't need all the fields returned
                Fields = new[] { "id", "title", "source", "score" },
                //enable hit highlighting
                Highlight = new HighlightingParameters()
                {
                    Fields = new[] { "articleBody", "abstract" }
                    ,
                    Fragsize = 200
                    ,
                    AfterTerm = "</em></strong>"
                    ,
                    BeforeTerm = "<em><strong>"
                    ,
                    UsePhraseHighlighter = true
                    //, AlternateField = "source"
                }
            };

            //issue the query
            var results = _ctx.Query(query, options);
            var highlights = results.Highlights;

            var resultCount = results.Highlights.Count;
            var searchResults = new List<QuoteHighlight>();
            for (int i = 0; i < resultCount; i++)
            {
                //get the basic document information before dealing with highlights
                var highlight = new QuoteHighlight()
                {
                    Id = results[i].Id,
                    Title = results[i].Title,
                    Source = results[i].Source,
                    Score = results[i].Score.Value
                };

                //highlights are a separate array, and can be an array of hits...
                foreach (var h in highlights[results[i].Id])
                {
                    highlight.ArticleBodySnippet += String.Join(",", h.Value.ToArray());
                }
                searchResults.Add(highlight);
            }
            totalResults = results.NumFound;
            return searchResults;
        }
    }
}
