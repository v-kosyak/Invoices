using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Invoices.Models;

namespace Invoices.Controllers
{
    public class InvoiceLineController : InvoicesBaseController
    {
        public IEnumerable<Product> Products
        {
            get
            {
                if (Invoice == null)
                    return CachedProducts;
                else
                {
                    var exceptions = Invoice.Lines.Select(line => line.ProductId);
                    return CachedProducts.Where(p => !exceptions.Contains(p.Number));
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
