using Core.Entities.Base;

namespace Core.Entities;

public class Customer : Person
{
    public string PhoneNumber { get; set; }
    public string PIN { get; set; }
    public string SeriaNumber { get; set; }
    public string Seria { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Product> Products { get; set; }

}