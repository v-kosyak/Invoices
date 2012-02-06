using System;
using System.Web.Mvc;
using System.Web.Routing;
using Invoices.Api;
using Invoices.Models;

namespace Invoices.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {

        public IAuthenticationService Service { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (Service == null) { Service = new AuthenticationService(); }
            if (MembershipService == null) { MembershipService = new Facade(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOn model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    Service.SignIn(model);
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Invoice");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            Service.SignOut();

            return RedirectToAction("LogOn", "Account");
        }

    }
}
