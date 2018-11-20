using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guldtand.Data.Entities
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12)]
        public string PIDNumber { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string Zip { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public bool HasInsurance { get; set; }
    }
}
