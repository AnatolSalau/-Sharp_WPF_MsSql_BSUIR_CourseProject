using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("Calculation")]
    public class Calculation
    {
        [Key]
        public int Id { get; set; }

        public int StructureId { get; set; }
        public virtual Structure Structure { get; set; }

        public int ExpenditureId { get; set; }
        public virtual Expenditure Expenditure { get; set; }

        public int NomenclatureId { get; set; }
        public virtual Nomenclature Nomenclature { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Summa { get; set; }
    }
}
