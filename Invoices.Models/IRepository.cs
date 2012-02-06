using System.Collections.Generic;

namespace Invoices.Models
{
    public interface IRepository
    {
        IEnumerable<Customer> GetAllCustomers();

        IEnumerable<Product> GetAllProducts();

        string Submit(Invoice invoice);

        byte[] GetPdf(string invoiceId);
    }
}
