using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("Nomenclature")]
    public class Nomenclature
    {
        public Nomenclature()
        {
            Expenses = new HashSet<Expense>();
            Calculations = new HashSet<Calculation>();
            Structures = new HashSet<Structure>();
            Outputs = new HashSet<Output>();
            Plans = new HashSet<Plan>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int NomenclatureTypeId { get; set; }
        public virtual NomenclatureType NomenclatureType { get; set; }

        public int ClassId { get; set; }
        public virtual Class Class { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        public virtual ICollection<Calculation> Calculations { get; set; }

        public virtual ICollection<Structure> Structures { get; set; }

        public virtual ICollection<Output> Outputs { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }
    }
}
