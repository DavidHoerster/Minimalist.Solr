using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Model;
using Agile.Minimalist.Repository;
using Microsoft.Practices.ServiceLocation;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Conventions;
using SolrNet;

namespace Agile.Minimalist.NancyRunner
{
    public class StaticBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            //set this up to define a directory of static assets
            // contains my JS, CSS, Images, etc.
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("/assets")
            );
        }

        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            //initialize my Solr service
            Console.WriteLine("Initializing Solr Search Service");
            Startup.Init<Quote>("http://localhost:8983/solr/historicalQuotes");
            Console.WriteLine("Search service initialized");


            //register some non-discoverable dependencies
            container
                .Register<IQuoteRepository>(new QuoteRepository(ServiceLocator.Current.GetInstance<ISolrOperations<Quote>>()));
            container.Register<IUserMapper, UserRepository>();


            //configure forms authentication
            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                {
                    RedirectUrl = "~/login",
                    UserMapper = container.Resolve<IUserMapper>(),
                };
            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);


            //silly logging statements
            pipelines.BeforeRequest += (ctx) =>
            {
                Console.WriteLine("starting up request, but set in app startup");
                return null;
            };

            pipelines.AfterRequest += (ctx) =>
            {
                Console.WriteLine("ending request, but set in app startup");
            };


            //wire everything else up automagically
            base.ApplicationStartup(container, pipelines);
        }

        //protected override void ConfigureRequestContainer(Nancy.TinyIoc.TinyIoCContainer container, NancyContext context)
        //{
        //    base.ConfigureRequestContainer(container, context);
        //}

        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            //more silly logging, but showing that maybe I can connect to
            // other systems before a request is executed.
            // Maybe do some logging, drop commands in a queue, etc.
            pipelines.BeforeRequest += (ctx) =>
            {
                Console.WriteLine("starting request for {0}", ctx.CurrentUser == null ? "Guest" : ctx.CurrentUser.UserName);
                Console.WriteLine("I could drop a command into a queue here for {0} {1}", context.Request.Method, context.Request.Url.ToString());
                return null;
            };
        }
    }
}
