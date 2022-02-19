using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("Expense")]
    public class Expense
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
        public decimal Summa { get; set; }

        public int ExpenditureId { get; set; }
        public virtual Expenditure Expenditure { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public int ClassId { get; set; }
        public virtual Class Class { get; set; }
    }
}
