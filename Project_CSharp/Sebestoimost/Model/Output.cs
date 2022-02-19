using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("Output")]
    public class Output
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int NomenclatureId { get; set; }
        public virtual Nomenclature Nomenclature { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [Required]
        public decimal Quantity { get; set; }
    }
}