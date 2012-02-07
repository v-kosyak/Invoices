using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Invoices.Models;

namespace Invoices.Controllers
{
    [HandleError(View = "Exception")]
    [Authorize]
    public abstract class InvoicesBaseController : Controller
    {
        private readonly Cached<Invoice> _cachedInvoice;
        private readonly Cached<IEnumerable<Product>> _cachedProducts;

        protected IEnumerable<Product> CachedProducts
        {
            get
            {
                return _cachedProducts.Value;
            }
            set
            {
                _cachedProducts.Value = value;
            }
        }

        public ICache Cache { get; set; }
        public IAuthenticationService AuthenticationService { get; set; }
        public IRepository Repository { get; set; }

        protected InvoicesBaseController()
        {
            _cachedInvoice = new Cached<Invoice>(() => Cache, () => new Invoice());
            _cachedProducts = new Cached<IEnumerable<Product>>(() => Cache, () => Repository.GetAllProducts())
                                  {Name = "Products"};
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (AuthenticationService == null) { AuthenticationService = new AuthenticationService(); }
            if (Repository == null) { Repository = new Api.Facade(AuthenticationService); }
            if (Cache == null) { Cache = new HttpSessionStateCache(Session); }
        }

        protected void ClearCache()
        {
            _cachedInvoice.Clear();
            _cachedProducts.Clear();
        }

        public Invoice Invoice
        {
            get { 
                return _cachedInvoice.Value;
            }
            set
            {
                _cachedInvoice.Value = value;
            }
        }
    }
}