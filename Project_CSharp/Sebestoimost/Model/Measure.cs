using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("Measure")]
    public class Measure
    {
        public Measure()
        {
            Classes = new HashSet<Class>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Name { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
    }
}
