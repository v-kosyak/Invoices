using TechTalk.SpecFlow;

namespace Invoices.Specs
{
    [Binding]
    public class StepDefinitions
    {
        [Given(@"I am authorized")]
        public void GivenIAmAuthorized()
        {
            ScenarioContext.Current.Pending();
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
