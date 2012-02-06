using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using Economic.Api;
using Economic.Api.Data;
using Invoices.Models;

namespace Invoices.Api
{
    public class Facade: IMembershipService, IRepository
    {
        private ISession _session;
        private readonly IAuthenticationService _authenticationService;

        private ISession _Session
        {
            get
            {
                if (_session == null)
                {
                    _session = new EconomicSession();
                }
                return _session;
            }
        }

        public ISession Session 
        { 
            get
            {
                try
                {
                    _Session.Connect(Properties.Settings.Default.ProjectId, _authenticationService.UserName,
                                     _authenticationService.Password);
                }
                catch(Exception e)
                {
                    throw new AuthenticationException(e.Message, e);
                }
                
                return _session;
            }
            set { _session = value; }
        }

        public Facade(IAuthenticationService authenticationService = null)
        {
            _authenticationService = authenticationService;
        }

        public bool ValidateUser(string userName, string password)
        {
            try
            {
                _Session.Connect(Properties.Settings.Default.ProjectId, userName, password);
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            IDebtor[] debtors = Session.Debtor.GetAll();

            foreach (var debtorData in Session.DebtorData.GetDataArray(debtors))
                if (debtorData.IsAccessible)
                {
                    yield return new Customer
                                       {
                                           Number = debtorData.Number,
                                           Name = debtorData.Name,
                                           Address = debtorData.Address
                                       };
                }
        }


        public IEnumerable<Product> GetAllProducts()
        {
            return ProductDatas.Select(productData => new Product { Number = productData.Number, Name = productData.Name });
        }

        private IEnumerable<IProductData> ProductDatas
        {
            get
            {
                var products = Session.Product.GetAll();

                foreach (var productData in Session.ProductData.GetDataArray(products))
                    if (productData.IsAccessible)
                    {
                        yield return productData;
                    }
            }
        }

        public string Submit(Invoice invoice)
        {
            var debtor = Session.Debtor.FindByNumber(invoice.CustomerId);

            var currentInvoice = Session.CurrentInvoice.Create(debtor);

            foreach (var invoiceLine in invoice.Lines)
            {
                var currentInvoiceLineData = Session.CurrentInvoiceLine.Create(currentInvoice);
                currentInvoiceLineData.Product = Session.Product.FindByNumber(invoiceLine.ProductId);
                currentInvoiceLineData.Quantity = invoiceLine.Quantity;
            }

            return currentInvoice.Book().Number.ToString();
        }

        public byte[] GetPdf(string invoiceId)
        {
            int invoiceNumber;

            if (!int.TryParse(invoiceId, out invoiceNumber))
                throw new ArgumentException("invoiceId");

            var invoice = Session.Invoice.FindByNumber(invoiceNumber);

            return invoice.GetPdf();
        }
    }
}
