using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using WatiN.Core;

namespace Invoices.Specs
{
    [Binding]
    public class StepDefinitions
    {
        private static Properties.Settings Settings
        {
            get { return Properties.Settings.Default; }
        }

        [Given(@"I am authorized")]
        public void GivenIAmAuthorized()
        {
            var url = Settings.BaseUrl;
            WebBrowser.Current.GoTo(url);

            var userName = WebBrowser.Current.TextField(Find.ByName("UserName"));
            Assert.IsNotNull(userName, "User Name");
            userName.TypeText(Settings.UserName);

            var password = WebBrowser.Current.TextField(Find.ByName("Password"));
            Assert.IsNotNull(password, "Password");
            password.TypeText(Settings.Password);

            WebBrowser.Current.Button(Find.ByValue("Log On")).Click();

            Assert.IsTrue(WebBrowser.Current.Link(Find.ByText("Log Off")).Exists);
        }

        [Given(@"I have selected customer with id=101")]
        public void GivenIHaveSelectedCustomerWithId101()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"addrees is address")]
        public void ThenAddreesIsAddress()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"customer with id=101 is selected")]
        public void ThenCustomerWithId101IsSelected()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
