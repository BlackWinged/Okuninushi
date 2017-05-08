using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Okunishushi.Filters
{
    public class AuthFilterAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new InternalAuthFIlter();
        }
        private class InternalAuthFIlter : IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (context.HttpContext.Session.GetString("currentuser") == null && !context.HttpContext.Request.Path.Value.Contains("login"))
                {
                    context.HttpContext.Response.Redirect("/classroom/login");
                }
            }
        }
    }
}
