using System;
using System.ComponentModel.DataAnnotations;

namespace Invoices.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UnsignedNonZeroAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            dynamic dynValue = value;

            return dynValue > 0;
        }
    }
}
