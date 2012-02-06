namespace Invoices.Models
{
    public interface ICache
    {
        object this[string name] { get; set; }
    }
}
