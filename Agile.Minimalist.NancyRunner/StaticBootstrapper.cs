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

            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("/assets")
            );
        }

        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            pipelines.BeforeRequest += (ctx) =>
            {
                Console.WriteLine("starting up application");
                return null;
            };

            pipelines.AfterRequest += (ctx) =>
            {
                Console.WriteLine("ending application");
            };

            container
                .Register<IQuoteRepository>(new QuoteRepository(ServiceLocator.Current.GetInstance<ISolrOperations<Quote>>()));

            base.ApplicationStartup(container, pipelines);
        }

        protected override void ConfigureRequestContainer(Nancy.TinyIoc.TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register<IUserMapper, UserRepository>();
        }

        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            pipelines.BeforeRequest += (ctx) =>
            {
                Console.WriteLine("starting request for {0}", ctx.CurrentUser == null ? "Guest" : ctx.CurrentUser.UserName);
                return null;
            };

            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                {
                    RedirectUrl = "~/login",
                    UserMapper = container.Resolve<IUserMapper>(),
                };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }
    }
}
