using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sebestoimost.Model
{
    [Table("Cost")]
    public class Cost
    {
        public Cost()
        {
            Structures = new HashSet<Structure>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Structure> Structures { get; set; }
    }
}

