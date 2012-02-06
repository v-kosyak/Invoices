namespace Invoices.Models
{
    public interface IAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
        string UserName { get; }
        string Password { get; }

        void SignIn(LogOn model);
    }
}
