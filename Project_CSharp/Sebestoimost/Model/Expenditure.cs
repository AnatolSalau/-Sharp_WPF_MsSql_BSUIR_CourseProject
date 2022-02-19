using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("Expenditure")]
    public class Expenditure
    {
        public Expenditure()
        {
            Expenses = new HashSet<Expense>();
            Calculations = new HashSet<Calculation>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        public virtual ICollection<Calculation> Calculations { get; set; }
    }
}
