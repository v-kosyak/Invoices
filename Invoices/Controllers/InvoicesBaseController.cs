using System;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Invoices.Models;

namespace Invoices.Controllers
{
    [HandleError(View = "Exception")]
    [Authorize]
    public abstract class InvoicesBaseController : Controller
    {
        public ICache Cache { get; set; }
        public IAuthenticationService AuthenticationService { get; set; }
        public IRepository Repository { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (AuthenticationService == null) { AuthenticationService = new AuthenticationService(); }
            if (Repository == null) { Repository = new Api.Facade(AuthenticationService); }
            if (Cache == null) { Cache = new HttpSessionStateCache(Session); }
        }

        protected void ClearCache()
        {
            Invoice = null;
            Cache["Products"] = null;
        }

        public Invoice Invoice
        {
            get { 
                if (Cache == null)
                    return null;

                var cachedInvoice = Cache["Invoice"] as Invoice;
                if (cachedInvoice == null)
                {
                    cachedInvoice = Invoice = new Invoice();
                }

                return cachedInvoice;
            }
            set { Cache["Invoice"] = value; }
        }
    }
}