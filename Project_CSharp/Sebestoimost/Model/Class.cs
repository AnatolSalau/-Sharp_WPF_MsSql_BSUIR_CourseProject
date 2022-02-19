using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("Class")]
    public class Class
    {
        public Class()
        {
            Expenses = new HashSet<Expense>();
            Nomenclatures = new HashSet<Nomenclature>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int MeasureId { get; set; }
        public virtual Measure Measure { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        public virtual ICollection<Nomenclature> Nomenclatures { get; set; }
    }
}
