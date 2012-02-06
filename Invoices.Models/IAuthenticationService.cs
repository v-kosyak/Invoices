namespace Invoices.Models
{
    public interface IAuthenticationService
    {
        void SignOut();
        string UserName { get; }
        string Password { get; }

        void SignIn(LogOn model);
    }
}
