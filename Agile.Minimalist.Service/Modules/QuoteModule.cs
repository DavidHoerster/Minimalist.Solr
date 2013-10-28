using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Model;
using Agile.Minimalist.Repository;
using Nancy;
using Nancy.ModelBinding;

namespace Agile.Minimalist.Modules
{
    public class QuoteModule : NancyModule
    {
        private readonly QuoteRepository _repo;
        public QuoteModule()
        {
            _repo = new QuoteRepository();

            Get["/quote"] = _ =>
            {
                return View["Index.cshtml", _repo.GetAll()];
            };

            Get["/quote/{id}"] = args =>
            {
                return View["details.cshtml", _repo.GetWithDetail(args.id)];
            };

            Get["/quote/create"] = _ =>
            {
                return View["create.cshtml"];
            };

            Post["/quote/create"] = args =>
            {
                var newQuote = this.Bind<Quote>();
                if (_repo.Insert(newQuote))
                {
                    return this.Response.AsRedirect("/quote");
                }
                else
                {
                    return View["index.cshtml"];
                }
            };

            Get["/quote/delete/{id}"] = args =>
            {
                _repo.DeleteById(args.id);
                return this.Response.AsRedirect("/quote");
            };

            Get["/quote/edit/{id}"] = args =>
            {
                return View["edit.cshtml", _repo.GetById(args.id)];
            };

            Post["/quote/edit/{id}"] = model =>
            {
                var theQuote = this.Bind<Quote>();
                if (_repo.Update(theQuote))
                {
                    return this.Response.AsRedirect("/quote");
                }
                else
                {
                    return "ooops!";
                }
            };

            Get["/quote/search"] = args =>
            {
                Int32 numFound;
                var searchResults = _repo.Search(this.Request.Query.id, out numFound);
                ViewBag.TotalResults = numFound;
                ViewBag.NumberShown = searchResults.Count;
                ViewBag.Term = this.Request.Query.id;
                return View["search.cshtml", searchResults];
            };
        }
    }
}
