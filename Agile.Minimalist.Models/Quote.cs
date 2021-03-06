﻿using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agile.Minimalist.Model
{
    public class Quote : BaseQuote
    {
        [SolrUniqueKey("id")]
        public String Id { get; set; }
        [SolrField("title")]
        public String Title { get; set; }
        [SolrField("articleBody")]
        public String ArticleBody { get; set; }
        [SolrField("year")]
        public Int32 Year { get; set; }
        [SolrField("abstract")]
        public String Abstract { get; set; }
        [SolrField("source")]
        public String Source { get; set; }
        [SolrField("score")]
        public Double? Score { get; set; }
    }
}