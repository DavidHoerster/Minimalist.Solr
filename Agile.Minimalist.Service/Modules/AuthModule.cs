using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Repository;
using Nancy;
using Nancy.Extensions;
using Nancy.Authentication.Forms;

namespace Agile.Minimalist.Service.Modules
{
    public class AuthModule : NancyModule
    {
        public AuthModule()
        {
            Get["/login"] = parameters =>
            {
                dynamic model = new ExpandoObject();
                model.Errored = this.Request.Query.error.HasValue;
                model.ReturnUrl = this.Request.Query.returnUrl;

                return View["login.cshtml", model];
            };

            Get["/logout"] = parameters =>
            {
                return this.LogoutAndRedirect("/quote");
            };

            Post["/login"] = p =>
            {
                var userId = UserRepository.ValidateUser(this.Request.Form.UserName, this.Request.Form.Password);
                if (userId == null)
                {
                    return this.Response.AsRedirect("~/login?error=true&username=" + (string)this.Request.Form.UserName);
                }

                var url = (String)this.Request.Form.ReturnUrl;
                if (String.IsNullOrWhiteSpace(url))
                {
                    url = "/quote";
                }
                return this.LoginAndRedirect(((Guid?)userId).Value, null, url);
            };
        }
    }
}
