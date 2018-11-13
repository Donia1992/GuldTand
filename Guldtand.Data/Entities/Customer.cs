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
        public virtual string FirstName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 2)]
        public virtual string LastName { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12)]
        public virtual string PIDNumber { get; set; }

        [Required]
        [Phone]
        public virtual string Phone { get; set; }

        [Required]
        public virtual string Email { get; set; }

        [Required]
        public virtual string Street { get; set; }

        [Required]
        public virtual string Zip { get; set; }

        [Required]
        public virtual string City { get; set; }

        [Required]
        public virtual bool HasInsurance { get; set; }
    }
}
