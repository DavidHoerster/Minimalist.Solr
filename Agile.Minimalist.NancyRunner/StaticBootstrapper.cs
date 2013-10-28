using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Repository;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Conventions;

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

        protected override void ConfigureRequestContainer(Nancy.TinyIoc.TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register<IUserMapper, UserRepository>();
        }

        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
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
