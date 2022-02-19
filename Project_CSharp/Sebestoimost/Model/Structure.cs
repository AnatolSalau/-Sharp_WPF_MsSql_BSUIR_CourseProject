using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("Structure")]
    public class Structure
    {
        public Structure()
        {
            Calculations = new HashSet<Calculation>();
        }

        [Key]
        public int Id { get; set; }

        public int CostId { get; set; }
        public virtual Cost Cost { get; set; }

        public int NomenclatureId { get; set; }
        public virtual Nomenclature Nomenclature { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        public virtual ICollection<Calculation> Calculations { get; set; }
    }
}