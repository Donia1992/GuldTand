using System.ComponentModel.DataAnnotations;

namespace Guldtand.Data.Entities
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
