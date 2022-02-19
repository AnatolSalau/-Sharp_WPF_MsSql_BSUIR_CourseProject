using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("Plan")]
    public class Plan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int NomenclatureId { get; set; }
        public virtual Nomenclature Nomenclature { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
    }
}