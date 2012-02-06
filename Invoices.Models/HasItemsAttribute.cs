using System;
using System.Linq;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Invoices.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class HasItemsAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is IEnumerable)
                return (value as IEnumerable).OfType<object>().Count() > 0;

            return base.IsValid(value);
        }
    }
}
