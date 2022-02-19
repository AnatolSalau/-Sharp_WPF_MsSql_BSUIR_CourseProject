using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("Role")]
    public class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
