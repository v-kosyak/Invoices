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

        [AfterScenario("closes_browser_afterwards")]
        public void CloseBrowserAfterScenario()
        {
            WebBrowser.Current.Close();
        }

        [Given(@"I am authorized")]
        public void GivenIAmAuthorized()
        {
            var url = Settings.BaseUrl;
            WebBrowser.Current.GoTo(url);

            var logOff = WebBrowser.Current.Link(Find.ByText("Log Off"));
            if (logOff.Exists)
                logOff.Click();

            var userName = WebBrowser.Current.TextField(Find.ByName("UserName"));
            Assert.IsTrue(userName.Exists, "User Name");
            userName.TypeText(Settings.UserName);

            var password = WebBrowser.Current.TextField(Find.ByName("Password"));
            Assert.IsTrue(password.Exists, "Password");
            password.TypeText(Settings.Password);

            WebBrowser.Current.Button(Find.ByValue("Log On")).Click();

            Assert.IsTrue(WebBrowser.Current.Link(Find.ByText("Log Off")).Exists);
        }

        [Given(@"I have selected customer with id=(.*)")]
        public void GivenIHaveSelectedCustomer(string customerId)
        {
            var customer = WebBrowser.Current.SelectList(Find.ByName("CustomerId"));
            customer.SelectByValue(customerId);
            StringAssert.StartsWith(customer.SelectedItem, customerId);
        }

        [Then(@"customer address is (.*)")]
        public void ThenCustomerAddressIsShown(string customerAddress)
        {
            var address = WebBrowser.Current.TextField(Find.ByName("CustomerAddress"));
            Assert.IsTrue(address.Exists, address.Name);

            Assert.AreEqual(customerAddress, address.Text);
        }

        [Then(@"customer with id=(.*) is selected")]
        public void ThenCustomerIsSelected(string customerId)
        {
            Assert.IsTrue(WebBrowser.Current.Url.Contains(string.Concat("CustomerId=", customerId)));
            var customer = WebBrowser.Current.SelectList(Find.ByName("CustomerId"));
            customer.SelectByValue(customerId);
            Assert.IsTrue(customer.Exists, customer.Name);
            StringAssert.StartsWith(customer.SelectedItem, customerId);
        }
    }
}
