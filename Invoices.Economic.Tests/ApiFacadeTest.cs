using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Economic.Api;
using Moq;

namespace Invoices.Api.Tests
{
    [TestClass]
    public class ApiFacadeTest
    {
        protected Mock<ISession> SessionMock { get; private set; }

        [TestInitialize]
        public void TestInitialize()
        {
            SessionMock = new Mock<ISession>();
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
    }
}
