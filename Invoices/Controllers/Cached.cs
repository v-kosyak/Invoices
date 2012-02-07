using System;
using Invoices.Models;

namespace Invoices.Controllers
{
    internal sealed class Cached<T> where T: class
    {
        private readonly Func<ICache> _cacheAccessor;
        private readonly Func<T> _factory;

        public string Name { get; set; }

        private ICache Cache
        {
            get { return _cacheAccessor(); }
        }

        private T CachedValue
        {
            get
            {
                if (Cache[Name] is T)
                    return (T)Cache[Name];
                return null;
            }
            set
            {
                Cache[Name] = value;
            }
        }

        public T Value
        {
            get
            {
                if (CachedValue == null)
                {
                    CachedValue = _factory();
                }

                return CachedValue;
            }
            set { CachedValue = value; }
        }

        public Cached(Func<ICache> cacheAccessor, Func<T> factory)
        {
            if (cacheAccessor == null)
                throw new ArgumentNullException("cacheAccessor");
            _cacheAccessor = cacheAccessor;

            if (factory == null)
                throw new ArgumentException("factory");

            _factory = factory;

            Name = typeof(T).Name;
        }

        public void Clear()
        {
            CachedValue = null;
        }

    }
}