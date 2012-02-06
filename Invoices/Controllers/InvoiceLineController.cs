using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Invoices.Models;

namespace Invoices.Controllers
{
    public class InvoiceLineController : InvoicesBaseController
    {
        private IEnumerable<Product> _products;

        private IEnumerable<Product> CachedProducts
        {
            get
            {
                if (Cache == null)
                    return null;

                return Cache["Products"] as IEnumerable<Product>;
            }
            set { Cache["Products"] = value; }
        }
        
        public IEnumerable<Product> Products
        {
            get
            {
                if (_products == null)
                {
                    if (CachedProducts == null)
                        CachedProducts = Repository.GetAllProducts();
                    _products = CachedProducts;
                }

                if (Invoice == null)
                    return _products;
                else
                {
                    var exceptions = Invoice.Lines.Select(line => line.ProductId);
                    return _products.Where(p => !exceptions.Contains(p.Number));
                }
            }
        }

        //
        // GET: /InvoiceLine/
        
        public ActionResult AddProduct()
        {
            ViewData["Products"] = GetProductSelectListItems();
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(InvoiceLine model)
        {
            if (ModelState.IsValid)
            {
                object customerId = null;

                if (Invoice != null)
                {
                    model.Product = Products.Single(product => product.Number == model.ProductId);
                    Invoice.AddLine(model);
                    customerId = new {CustomerId = Invoice.CustomerId};
                }

                return RedirectToAction("Create", "Invoice", customerId );
            }
            ViewData["Products"] = GetProductSelectListItems();
            return View(model);
        }

        private IEnumerable<SelectListItem> GetProductSelectListItems()
        {
            yield return new SelectListItem { Selected = true, Text = "-- Select a product --", Value = String.Empty };

            foreach (var product in Products)
            {
                var item = new SelectListItem();
                item.Text = product.Number + "-" + product.Name;
                item.Value = product.Number;

                yield return item;
            }

        }
    }
}
