namespace Guldtand.Domain.Models
{
    public interface ICustomer
    {
        int Uid { get; set; }
        string Firstname { get; set; }
        string Lastname { get; set;  }
        string Socialsecurity_number { get; set;  }
        string Phone { get; set; }
        string Email { get; set; }
        string Street { get; set; }
        string Zip { get; set; }
        string City { get; set; }
        bool Insurance { get; set; }
    }   
}
