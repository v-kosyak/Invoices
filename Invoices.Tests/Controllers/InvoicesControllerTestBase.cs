using Invoices.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Invoices.Tests.Controllers
{
    [TestClass]
    public class InvoicesControllerTestBase
    {
        protected Mock<IRepository> RepositoryMock { get; private set; }
        protected Mock<ICache> CacheMock { get; private set; }

        [TestInitialize]
        public void Setup()
        {
            var products = new[]
                                {
                                    new Product {Number = "1", Name = "Product1"},
                                    new Product {Number = "2", Name = "Product2"},
                                    new Product {Number = "3", Name = "Product3"}
                                };

            var customers = new[]
                                {
                                    new Customer {Number = "1", Name = "Cust1"},
                                    new Customer {Number = "2", Name = "Cust2"},
                                    new Customer {Number = "3", Name = "Cust3"}
                                };

            RepositoryMock = new Mock<IRepository>();
            RepositoryMock.Setup(rep => rep.GetAllProducts()).Returns(products);
            RepositoryMock.Setup(rep => rep.GetAllCustomers()).Returns(customers);

            CacheMock = new Mock<ICache>();

            var invoice = new Invoice();
            invoice.AddLine(new InvoiceLine());

            CacheMock.SetupGet(cache => cache["Invoice"]).Returns(invoice);
            CacheMock.SetupGet(cache => cache["Products"]).Returns(products);
        }
    }
}
