﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Minimalist.Model
{
    public class QuoteList : BaseQuote
    {
        public IEnumerable<Quote> Quotes { get; set; }
    }
}
