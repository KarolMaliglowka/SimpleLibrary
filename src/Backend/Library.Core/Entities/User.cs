using System.Text.RegularExpressions;
using Library.Core.ValueObjects;

namespace Library.Core.Entities;

public sealed class User : BaseClass
{
    public Name Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
    public List<Borrow> Borrows { get; set; } = [];
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public string FullName => $"{Name.Value} {Surname}";
}