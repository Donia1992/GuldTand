namespace Guldtand.Domain.Models
{
    public interface IEmployee
    {
        string Password { get; set; }
        int Uid { get; set; }
        string Name { get; set; }
    }
}