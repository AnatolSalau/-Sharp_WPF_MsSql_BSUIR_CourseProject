using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("Department")]
    public class Department
    {
        public Department()
        {
            Expenses = new HashSet<Expense>();
            Structures = new HashSet<Structure>();
            Outputs = new HashSet<Output>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Structure> Structures { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        public virtual ICollection<Output> Outputs { get; set; }
    }
}