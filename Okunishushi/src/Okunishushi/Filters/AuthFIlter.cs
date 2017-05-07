using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Okunishushi.Filters
{
    public class AuthFIlter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Path.Value.ToLower().Contains("classroom"))
            {
                context.HttpContext.Response.Redirect("classroom");

            }
        }
    }
}
