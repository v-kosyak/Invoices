using System;
using System.Web;
using System.Web.Security;

namespace Invoices.Models
{
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.
    public class AuthenticationService : IAuthenticationService
    {
        public string UserName
        {
            get { return HttpContext.Current.Session["UserName"] as string; }
            //get { return "CES"; }
            private set
            {
                HttpContext.Current.Session["UserName"] = value;
            }

        }

        public string Password
        {
            get { return HttpContext.Current.Session["Password"] as string; }
            //get { return "c97jxsq8"; }
            private set
            {
                HttpContext.Current.Session["Password"] = value;
            }

        }

        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
            UserName = null;
            Password = null;
        }

        public void SignIn(LogOn model)
        {
            this.SignIn(model.UserName, model.RememberMe);
            UserName = model.UserName;
            Password = model.Password;
        }
    }

}
