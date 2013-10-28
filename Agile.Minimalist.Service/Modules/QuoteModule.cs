using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Model;
using Agile.Minimalist.Repository;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

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
                var quotes = _repo.GetAll();
                SetUserName(quotes);
                return View["Index.cshtml", quotes];
            };

            Get["/quote/{id}"] = args =>
            {
                var quote = _repo.GetWithDetail(args.id);
                SetUserName(quote);
                return View["details.cshtml", quote];
            };

            Get["/quote/create"] = _ =>
            {
                this.RequiresAuthentication();

                return View["create.cshtml"];
            };

            Post["/quote/create"] = args =>
            {
                this.RequiresAuthentication();

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
                this.RequiresAuthentication();
                
                _repo.DeleteById(args.id);
                return this.Response.AsRedirect("/quote");
            };

            Get["/quote/edit/{id}"] = args =>
            {
                this.RequiresAuthentication();

                var quote = _repo.GetById(args.id);
                SetUserName(quote);
                return View["edit.cshtml", quote];
            };

            Post["/quote/edit/{id}"] = model =>
            {
                this.RequiresAuthentication();

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

        private void SetUserName(BaseQuote theObject)
        {
            if (this.Context.CurrentUser == null)
            {
                theObject.UserName = String.Empty;
            }
            else
            {
                theObject.UserName = this.Context.CurrentUser.UserName;
            }
        }
    }
}
