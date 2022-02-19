using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("NomenclatureType")]
    public class NomenclatureType
    {
        public NomenclatureType()
        {
            Nomenclatures = new HashSet<Nomenclature>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public virtual ICollection<Nomenclature> Nomenclatures { get; set; }
    }
}
