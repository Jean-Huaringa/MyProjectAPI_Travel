using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyProjectAPI_Travel.Models
{
    public class Seat
    {
        [Key]
        [Required]
        [Column("id_bus")]
        public int IdBus { get; set; }

        [Key]
        [Required]
        [Column("row")]
        public int Row { get; set; }

        [Key]
        [Required]
        [Column("column")]
        public int Column { get; set; }

        [Column("tipo")]
        [StringLength(20)]
        public string Type { get; set; } = null!;

        [Column("busy")]
        [Required]
        public bool? Busy { get; set; } // ocupado

        [Column("state")]
        [Required]
        public bool? State { get; set; }

        [ForeignKey("IdBus")]
        public virtual Bus Bus { get; set; } = null!;
    }
}
