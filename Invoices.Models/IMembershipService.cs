namespace Invoices.Models
{
    public interface IMembershipService
    {
        bool ValidateUser(string userName, string password);
    }
}
