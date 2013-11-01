using System;
namespace Agile.Minimalist.Repository
{
    public interface IQuoteRepository
    {
        void DeleteById(string quoteId);
        Agile.Minimalist.Model.QuoteList GetAll();
        Agile.Minimalist.Model.Quote GetById(string quoteId);
        Agile.Minimalist.Model.QuoteDetail GetWithDetail(string quoteId);
        bool Insert(Agile.Minimalist.Model.Quote theQuote);
        System.Collections.Generic.IEnumerable<Agile.Minimalist.Model.QuoteHighlight> Search(string searchKey, out int totalResults);
        bool Update(Agile.Minimalist.Model.Quote theQuote);
    }
}
