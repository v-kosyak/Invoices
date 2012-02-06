using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Invoices.Models;

namespace Invoices.Controllers
{
    [HandleError]
    [Authorize]
    public class InvoiceController : InvoicesBaseController
    {
        private IEnumerable<Customer> _customers;
        
        public ActionResult Index()
        {
            ClearCache();
            return RedirectToAction("Create");
        }

        private void InitializeCreate(string customerId)
        {
            var customer = Customers.SingleOrDefault(cust => cust.Number == customerId);

            ViewData.Add("Customers", CustomerSelectListItems);

            if (customer != null)
            {
                Invoice.CustomerId = customer.Number;
                Invoice.CustomerAddress = customer.Address;
            }
            else
            {
                Invoice.CustomerId = null;
                Invoice.CustomerAddress = null;
            }

            ViewData.Model = Invoice;
        }

        public ActionResult Create(string customerId = null)
        {
            InitializeCreate(customerId);
            return View(Invoice);
        }

        [HttpPost]
        public ActionResult Create(Invoice model)
        {
            return RedirectToAction("Create", new {CustomerId = model.CustomerId});
        }

        [HttpPost]
        public ActionResult Submit(Invoice model)
        {
            if (ModelState.IsValidField("CustomerId") && Invoice.Lines.Count() > 0)
            {
                try
                {
                    ViewData["InvoiceId"] = Repository.Submit(Invoice);
                    ClearCache();
                    return View("Submitted");
                }
                catch (Exception exception)
                {
                    ViewData.Model = new HandleErrorInfo(exception, "Invoice", "Submit");
                    return View("Exception");
                }
                
            }

            InitializeCreate(model.CustomerId);
            return View("Create", Invoice);
        }

        public ActionResult RemoveProduct(string productId)
        {
            var invoiceLineToRemove = Invoice.Lines.SingleOrDefault(line => line.ProductId == productId);
            if (invoiceLineToRemove != null)
                Invoice.RemoveLine(invoiceLineToRemove);

            return RedirectToAction("Create", new {CustomerId = Invoice.CustomerId});
        }

        public ActionResult GetPdf(string invoiceId)
        {
            var pdfFileContents = Repository.GetPdf(invoiceId);

            return new FileContentResult(pdfFileContents, "application/pdf");
        }

        protected IEnumerable<Customer> Customers
        {
            get {
                if (_customers == null)
                    _customers = Repository.GetAllCustomers();

                return _customers; 
            }
        }

        protected IEnumerable<SelectListItem> CustomerSelectListItems
        {
            get
            {
                yield return new SelectListItem { Selected = true, Text = "-- Select a customer --", Value = string.Empty };

                foreach (var customer in Customers)
                {
                    var item = new SelectListItem();
                    item.Text = customer.Number + "-" + customer.Name;
                    item.Value = customer.Number;

                    yield return item;
                }
            }
        }
    }
}
