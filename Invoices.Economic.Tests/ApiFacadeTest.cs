using System;
using System.Security.Authentication;
using Invoices.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Economic.Api;
using Moq;

namespace Invoices.Api.Tests
{
    [TestClass]
    public class ApiFacadeTest
    {
        protected Mock<ISession> SessionMock { get; private set; }
        protected Mock<IDebtorUtil> DebtorUtilMock { get; private set; }
        protected Mock<ICurrentInvoiceUtil> CurInvoiceUtilMock { get; private set; }
        protected Mock<ICurrentInvoiceLineUtil> CurInvoiceLineUtilMock { get; private set; }
        protected Mock<IProductUtil> ProductUtilMock { get; private set; }


        [TestInitialize]
        public void TestInitialize()
        {
            SessionMock = new Mock<ISession>();

            DebtorUtilMock = new Mock<IDebtorUtil>();
            SessionMock.SetupGet(session => session.Debtor).Returns(DebtorUtilMock.Object);

            ProductUtilMock = new Mock<IProductUtil>();
            SessionMock.SetupGet(session => session.Product).Returns(ProductUtilMock.Object);

            SetupCurInvoiceUtilMock();

            SetupCurInvoiceLineUtilMock();
        }

        private void SetupCurInvoiceUtilMock()
        {
            var invoiceMock = new Mock<IInvoice>();
            invoiceMock.SetupGet(invoice => invoice.Number).Returns(Int32.MaxValue);

            var curInvoiceMock = new Mock<ICurrentInvoice>();
            curInvoiceMock.Setup(curInvoice => curInvoice.Book()).Returns(invoiceMock.Object);

            CurInvoiceUtilMock = new Mock<ICurrentInvoiceUtil>();
            CurInvoiceUtilMock.Setup(util => util.Create(It.IsAny<IDebtor>())).Returns(curInvoiceMock.Object);

            SessionMock.SetupGet(session => session.CurrentInvoice).Returns(CurInvoiceUtilMock.Object);
        }

        private void SetupCurInvoiceLineUtilMock()
        {
            CurInvoiceLineUtilMock =new Mock<ICurrentInvoiceLineUtil>();
            CurInvoiceLineUtilMock.Setup(util => util.Create(It.IsAny<ICurrentInvoice>())).Returns(
                new Mock<ICurrentInvoiceLine>().Object);
            SessionMock.SetupGet(sessionMock => sessionMock.CurrentInvoiceLine).Returns(CurInvoiceLineUtilMock.Object);
        }

        [TestMethod]
        public void ValidateUserInvokesConnectOnSession()
        {
            //Arrange
            var expectedProjectId = Api.Properties.Settings.Default.ProjectId;

            var sut = new Facade{Session = SessionMock.Object};
            
            //Act
            var actual = sut.ValidateUser("user", "password");

            //Assert
            Assert.IsTrue(actual);

            SessionMock.Verify(session => session.Connect(expectedProjectId, "user", "password"));

        }

        [TestMethod]
        public void ValidateUserFailsOnException()
        {
            //Arrange
            var expectedProjectId = Api.Properties.Settings.Default.ProjectId;

            SessionMock.Setup(session => session.Connect(expectedProjectId, "user", "password")).Throws(new Exception());

            var sut = new Api.Facade { Session = SessionMock.Object };

            //Act
            var actual = sut.ValidateUser("user", "password");

            //Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPdfFailsIdInvoiceIdIsNotValidInteger()
        {
            //Arrange
            var sut = new Facade{Session = SessionMock.Object};

            //Act
            sut.GetPdf("not an integer");

        }

        [TestMethod]
        public void GetPdfReturnsPdfFileContents()
        {
            //Arrange
            var expectedBytes = new byte[255];

            var invoiceMock = new Mock<IInvoice>();
            invoiceMock.Setup(invoice => invoice.GetPdf()).Returns(expectedBytes);

            var invoiceUtilMock = new Mock<IInvoiceUtil>();
            invoiceUtilMock.Setup(util => util.FindByNumber(5)).Returns(invoiceMock.Object);

            SessionMock.SetupGet(session => session.Invoice).Returns(invoiceUtilMock.Object);

            var sut = new Facade {Session = SessionMock.Object};

            //Act
            var actual = sut.GetPdf("5");

            //Assert
            Assert.AreEqual(expectedBytes, actual);
        }

        [TestMethod]
        public void SubmitReturnsNumberOfBookedInvoice()
        {
            //Arrange
            const string expectedNumber = "13";

            var invoiceMock = new Mock<IInvoice>();
            invoiceMock.SetupGet(inv => inv.Number).Returns(13);

            var curInvoiceMock = new Mock<ICurrentInvoice>();
            curInvoiceMock.Setup(curInvoice => curInvoice.Book()).Returns(invoiceMock.Object);

            CurInvoiceUtilMock.Setup(util => util.Create(It.IsAny<IDebtor>())).Returns(curInvoiceMock.Object);

            var sut = new Facade {Session = SessionMock.Object};

            //Act
            var actual = sut.Submit(new Invoice());

            //Assert
            Assert.AreEqual(expectedNumber, actual);
        }

        [TestMethod]
        public void SubmitCreatesCurrentInvoiceWithDebtor()
        {
            //Arrange
            var debtorMock = new Mock<IDebtor>();
            DebtorUtilMock.Setup(debtor => debtor.FindByNumber("20")).Returns(debtorMock.Object);

            var sut = new Facade { Session = SessionMock.Object };

            //Act
            sut.Submit(new Invoice{CustomerId = "20"});

            //Assert
            CurInvoiceUtilMock.Verify(currentInvoice => currentInvoice.Create(debtorMock.Object));
        }

        [TestMethod]
        public void SubmitCreatesCurrentInvoiceLines()
        {
            //Arrange
            var invoice = new Invoice();
            invoice.AddLine(new InvoiceLine());
            invoice.AddLine(new InvoiceLine());
            invoice.AddLine(new InvoiceLine());

            var sut = new Facade { Session = SessionMock.Object };

            //Act
            sut.Submit(invoice);

            //Assert
            CurInvoiceLineUtilMock.Verify(currentInvoice => currentInvoice.Create(It.IsAny<ICurrentInvoice>()), Times.Exactly(3));
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void SessionIsNotUsableIfNotConnected()
        {
            //Arrange
            SessionMock.Setup(session => session.Connect(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());

            var sut = new Facade{Session = SessionMock.Object, Authentication = new Mock<IAuthenticationService>().Object};

            //Act
            var actual = sut.Session;
        }

        [TestMethod]
        public void SessionConnectsOnlyOnce()
        {
            //Arrange
            SessionMock.Setup(session => session.Connect(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns("connection");

            var sut = new Facade { Session = SessionMock.Object, Authentication = new Mock<IAuthenticationService>().Object };

            //Act
            var actual = sut.Session;
            actual = sut.Session;

            //Assert
            SessionMock.Verify(session => session.Connect(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),
                               Times.Once());
        }

    }
}
