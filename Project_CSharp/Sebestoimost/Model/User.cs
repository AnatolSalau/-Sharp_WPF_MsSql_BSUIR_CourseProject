using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("User")]
    public class User
    {
        public User()
        {
            Costs = new HashSet<Cost>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public bool Enabled { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
    }
}
