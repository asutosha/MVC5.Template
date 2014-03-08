﻿using System;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Components.Security;
using Template.Data.Core;

namespace Template.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected IRoleProvider RoleProvider
        {
            get;
            set;
        }

        public BaseController()
        {
        }

        protected virtual ActionResult RedirectToLocal(String url)
        {
            if (Url.IsLocalUrl(url))
                return Redirect(url);

            return RedirectToDefault();
        }
        protected virtual RedirectToRouteResult RedirectToDefault()
        {
            return RedirectToAction(String.Empty, String.Empty, new { language = RouteData.Values["language"], area = String.Empty });
        }
        protected virtual RedirectToRouteResult RedirectToUnauthorized()
        {
            return RedirectToAction("Unauthorized", new { language = RouteData.Values["language"], area = String.Empty, controller = "Home" });
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            String area = (String) filterContext.RouteData.Values["area"];
            String action = (String)filterContext.RouteData.Values["action"];
            String controller = (String)filterContext.RouteData.Values["controller"];

            if (!IsAuthorizedFor(area, controller, action))
                filterContext.Result = RedirectToUnauthorized();
        }
        protected Boolean IsAuthorizedFor(String action)
        {
            if (RoleProvider == null) return true;
            return RoleProvider.IsAuthorizedForAction(action);
        }
        protected Boolean IsAuthorizedFor(String area, String controller, String action)
        {
            if (RoleProvider == null) return true;
            return RoleProvider.IsAuthorizedForAction(area, controller, action);            
        }
    }
}
