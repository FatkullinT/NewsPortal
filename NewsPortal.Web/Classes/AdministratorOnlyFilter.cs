using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using NewsPortal.Domain.Security;

namespace NewsPortal.Web.Classes
{
    public class AdministratorOnlyFilter : IAuthorizationFilter
    {
        private IAuthenticationService _authenticationService;
        private IRoleProvider _roleProvider;

        public AdministratorOnlyFilter(IAuthenticationService authenticationService, IRoleProvider roleProvider)
        {
            _authenticationService = authenticationService;
            _roleProvider = roleProvider;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!_roleProvider.IsUserInRole(_authenticationService.CurrentUserId, "Administrator"))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {
                        "action", "Index"
                    },
                    {
                        "controller", "Home"
                    }
                });
            }
        }
    }
}