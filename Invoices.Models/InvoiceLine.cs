using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Invoices.Models
{
    public class InvoiceLine
    {
        [Required]
        [DisplayName("Product")]
        public string ProductId { get; set; }

        [Required]
        [UnsignedNonZero(ErrorMessage = "Quantity has to be more than a zero")]
        public decimal Quantity { get; set; }

        public Product Product { get; set; }
    }
}
