using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Invoices.Controllers;
using Invoices.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Invoices.Tests.Controllers
{
    [TestClass]
    public class InvoiceLineControllerTest: InvoicesControllerTestBase
    {
        [TestMethod]
        public void AddProductPopulatesProductSelectItems()
        {
            //Arrange
            var sut = new InvoiceLineController
                          {
                              Cache = CacheMock.Object
                          };

            //Act
            var actual = sut.AddProduct()  as ViewResult;
            
            //Arrange
            Assert.IsNotNull(actual);

            var productSelectList = actual.ViewData["Products"] as IEnumerable<SelectListItem>;

            Assert.IsNotNull(productSelectList);
            Assert.AreEqual(productSelectList.Single(item => item.Value == "1").Text, "1-Product1");
            Assert.AreEqual(productSelectList.Single(item => item.Value == "2").Text, "2-Product2");
            Assert.AreEqual(productSelectList.Single(item => item.Value == "3").Text, "3-Product3");
        }

        [TestMethod]
        public void AddProductRedirectsToInvoiceCreateIfModelValid()
        {
            //Arange
            CacheMock.SetupGet(cache => cache["Invoice"]).Returns(new Invoice {CustomerId = "8"});
            var sut = new InvoiceLineController{Cache = CacheMock.Object};

            //Act
            var actual = sut.AddProduct(new InvoiceLine{ProductId = "1"}) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual("Create",actual.RouteValues["action"]);
            Assert.AreEqual("Invoice", actual.RouteValues["controller"]);
            Assert.AreEqual("8", actual.RouteValues["CustomerId"]);
        }

        [TestMethod]
        public void ProductsAreNotFetchedFromRepsitoryIfCached()
        {
            //Arrange
            var expected = new Product {Number = "7"};

            CacheMock.SetupGet(state => state["Products"]).Returns(new[] {expected});
            
            var sut = new InvoiceLineController {Repository = RepositoryMock.Object, Cache = CacheMock.Object};

            //Act
            var actual = sut.Products;

            //Assert
            RepositoryMock.Verify(rep => rep.GetAllProducts(), Times.Never());

            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(expected, actual.First());

        }

        [TestMethod]
        public void AddProductAddInvoiceLineToInvoice()
        {
            //Arange
            var expected = new InvoiceLine{ProductId = "3"};

            var sut = new InvoiceLineController{Cache = CacheMock.Object};

            //Act
            sut.AddProduct(expected);

            //Assert
            Assert.IsTrue(sut.Invoice.Lines.Contains(expected));
            Assert.IsNotNull(expected.Product);
            Assert.AreEqual("3", expected.Product.Number);
            
        }

        [TestMethod]
        public void ProductsDoesNotContainAlreadyAddedToInvoice()
        {
            //Arrange
            var invoice = new Invoice();
            invoice.AddLine(new InvoiceLine {ProductId = "3"});
            invoice.AddLine(new InvoiceLine { ProductId = "4" });

            CacheMock.SetupGet(cache => cache["Invoice"]).Returns(invoice);

            CacheMock.SetupGet(cache => cache["Products"]).Returns(new[]
                                                                          {
                                                                              new Product {Number = "2"},
                                                                              new Product {Number = "3"}
                                                                          });

            var sut = new InvoiceLineController { Cache =  CacheMock.Object};

            //Act
            var actual = sut.Products;

            //Assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual("2", actual.First().Number);

        }

        [TestMethod]
        public void ProductsAreFetchedAndPutIntoCacheifNotCached()
        {
            //Arrange
            var expectedProducts = new[]
                                       {
                                           new Product {Number = "1", Name = "Product1"},
                                           new Product {Number = "2", Name = "Product2"}
                                       };

            RepositoryMock.Setup(rep => rep.GetAllProducts()).Returns(expectedProducts);
            CacheMock.SetupSequence(cache => cache["Products"])
                .Returns(null)
                .Returns(expectedProducts);

            var sut = new InvoiceLineController { Repository = RepositoryMock.Object, Cache = CacheMock.Object };

            //Act
            var actual1 = sut.Products;
            var actual2 = sut.Products;

            //Assert
            RepositoryMock.Verify(rep => rep.GetAllProducts(), Times.Once());

            CacheMock.VerifySet(cache => cache["Products"] = expectedProducts);

            CollectionAssert.AreEqual(actual1.ToArray(), actual2.ToArray());
        }
    }
}
