using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Guldtand.Data
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
        public virtual string PersonalNumber { get; set; }

        [Required]
        [Phone]
        public virtual string PhoneNumber { get; set; }

        [Required]
        public virtual bool HasInsurance { get; set; }

    }
}
