using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Invoices.Models
{
    public class Invoice
    {
        private IList<InvoiceLine> _invoiceLines = new List<InvoiceLine>();

        [Required]
        [DisplayName("Customer")]
        public string CustomerId { get; set; }

        [DisplayName("Address")]
        public string CustomerAddress { get; set; }

        [Required]
        [HasItems(ErrorMessage = "At least one product has to be added")]
        public IEnumerable<InvoiceLine> Lines
        {
            get { return _invoiceLines; }
        }

        public void AddLine(InvoiceLine invoiceLine)
        {
            _invoiceLines.Add(invoiceLine);
        }

        public void RemoveLine(InvoiceLine invoiceLine)
        {
            _invoiceLines.Remove(invoiceLine);
        }
    }
}
