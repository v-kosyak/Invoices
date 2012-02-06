using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Invoices.Controllers;
using Invoices.Models;
using Moq;

namespace Invoices.Tests.Controllers
{
    [TestClass]
    public class InvoiceControllerTest: InvoicesControllerTestBase
    {
        [TestMethod]
        public void IndexRedirectsToCreateAction()
        {
            // Arrange
            var sut = new InvoiceController{Cache = CacheMock.Object};

            // Act
            var actual = sut.Index() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual("Create", actual.RouteValues["action"]);
        }

        [TestMethod]
        public void CreatePopulatesCustomerSelectItemList()
        {
            //Arrange
            var sut = new InvoiceController {Repository = RepositoryMock.Object, Cache = CacheMock.Object};

            //Act
            var actual = sut.Create() as ViewResult;

            //Arrange
            Assert.IsNotNull(actual);

            var customerSelectList = actual.ViewData["Customers"] as IEnumerable<SelectListItem>;

            Assert.IsNotNull(customerSelectList);
            Assert.AreEqual(customerSelectList.Single(item => item.Value == "1").Text, "1-Cust1");
            Assert.AreEqual(customerSelectList.Single(item => item.Value == "2").Text, "2-Cust2");
            Assert.AreEqual(customerSelectList.Single(item => item.Value == "3").Text, "3-Cust3");

        }

        [TestMethod]
        public void CreatePopulatesCustomerSelectItemListWithEmptyValue()
        {
            //Arrange
            var sut = new InvoiceController { Repository = RepositoryMock.Object, Cache = CacheMock.Object };

            //Act
            var actual = sut.Create() as ViewResult;

            //Arrange
            Assert.IsNotNull(actual);
            
            //Assert
            var customerSelectList = actual.ViewData["Customers"] as IEnumerable<SelectListItem>;
            Assert.IsNotNull(customerSelectList);

            Assert.AreEqual(customerSelectList.Single(item => String.IsNullOrEmpty(item.Value)).Text, "-- Select a customer --");
            Assert.IsTrue(customerSelectList.Single(item => String.IsNullOrEmpty(item.Value)).Selected);
        }

        [TestMethod]
        public void CreateExposesCustomerAddressIfSelected()
        {
            //Arrange
            const string expectedCustomerId = "5";
            const string expectedCustomerAddress = "Address5";
            RepositoryMock.Setup(rep => rep.GetAllCustomers()).Returns(new[]
                                                                           {
                                                                               new Customer
                                                                                   {Number = expectedCustomerId, Address = expectedCustomerAddress}
                                                                           });
            var sut = new InvoiceController { Repository = RepositoryMock.Object, Cache = CacheMock.Object};

            //Act
            var viewResult = sut.Create(expectedCustomerId) as ViewResult;

            //Assert
            var actual = viewResult.ViewData.Model as Invoice;

            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedCustomerId, actual.CustomerId);
            Assert.AreEqual(expectedCustomerAddress, actual.CustomerAddress);
        }

        [TestMethod]
        public void GetPdfExtractsBytesByInvoiceId()
        {
            //Arrange
            var expectedBytes = new byte[255];

            RepositoryMock.Setup(rep => rep.GetPdf("7")).Returns(expectedBytes);

            var sut = new InvoiceController {Repository = RepositoryMock.Object};

            //Act
            var actual = sut.GetPdf("7") as FileContentResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedBytes, actual.FileContents);
            Assert.AreEqual("application/pdf", actual.ContentType);
        }

        [TestMethod]
        public void RemoveProductFromLinesAndRedirectsToCreate()
        {
            //Arrange
            var invoice = new Invoice();
            invoice.AddLine(new InvoiceLine { ProductId = "5" });
            invoice.AddLine(new InvoiceLine{ProductId = "8"});

            CacheMock.SetupGet(cache => cache["Invoice"]).Returns(invoice);

            var sut = new InvoiceController {Cache = CacheMock.Object};

            //Act
            var actual = sut.RemoveProduct("5") as RedirectToRouteResult;
            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual("Create", actual.RouteValues["action"]);

            Assert.AreEqual(1, invoice.Lines.Count());
            Assert.AreEqual("8", invoice.Lines.First().ProductId);
        }

        [TestMethod]
        public void SubmitHandlesExceptions()
        {
            //Arrange

            var expectedException = new Exception();
            RepositoryMock.Setup(rep => rep.Submit(It.IsAny<Invoice>())).Throws(expectedException);

            var sut = new InvoiceController{Repository = RepositoryMock.Object, Cache = CacheMock.Object};

            //Act
            var actual = sut.Submit(new Invoice()) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual("Exception", actual.ViewName);

            var actualErrorInfo = actual.ViewData.Model as HandleErrorInfo;
            Assert.IsNotNull(actualErrorInfo);
            Assert.AreEqual(expectedException, actualErrorInfo.Exception);
            Assert.AreEqual("Invoice", actualErrorInfo.ControllerName);
            Assert.AreEqual("Submit", actualErrorInfo.ActionName);
        }

    }
}
