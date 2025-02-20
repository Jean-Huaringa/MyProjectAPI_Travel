using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyProjectAPI_Travel.Models
{
    public class Itinerary
    {
        [Key]
        [Required]
        [Column("id_itn")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdItn { get; set; }

        [Column("id_origin")]
        [Required]
        public int IdOrigin { get; set; }

        [Column("id_destination")]
        [Required]
        public int IdDestination { get; set; }

        [Column("id_wrk")]
        [Required]
        public int IdWrk { get; set; }

        [Column("id_bus")]
        [Required]
        public int IdBus { get; set; }

        [Column("start_date", TypeName = "datetime")]
        [Required]
        public DateTime StartDate { get; set; }

        [Column("arrival_date", TypeName = "datetime")]
        [Required]
        public DateTime ArrivalDate { get; set; } // Fecha de llegada

        [Column("price", TypeName = "decimal(10, 2)")]
        [Required]
        public decimal Price { get; set; }

        [Column("availability")]
        [Required]
        public bool Availability { get; set; } // disponibilidad

        [Column("state")]
        [Required]
        public bool State { get; set; }

        [ForeignKey("IdBus")]
        public virtual Bus Bus { get; set; } = null!;

        [ForeignKey("IdDestination")]
        public virtual Station Destination { get; set; } = null!;

        [ForeignKey("IdOrigin")]
        public virtual Station Origin { get; set; } = null!;

        [ForeignKey("IdWrk")]
        public virtual Worker Worker { get; set; } = null!;

    }
}
