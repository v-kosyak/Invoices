using System;
using System.Threading;
using System.Web;

namespace Invoices.Controllers
{
    internal sealed class HttpSessionStateCache: Models.ICache
    {
        private HttpSessionStateBase _sessionState;

        public HttpSessionStateCache(HttpSessionStateBase sessionState)
        {
            if (sessionState == null)
                throw new ArgumentNullException("sessionState");

            _sessionState = sessionState;
        }

        public object this[string name]
        {
            get
            {
                return _sessionState[name];
            }
            set { _sessionState[name] = value; }
        }
    }
}