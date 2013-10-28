using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agile.Minimalist.Model
{
    public class QuoteHighlight : BaseQuote
    {
        public String Id { get; set; }
        public String Title { get; set; }
        public String ArticleBodySnippet { get; set; }
        public String Source { get; set; }
        public Double Score { get; set; }
    }
}