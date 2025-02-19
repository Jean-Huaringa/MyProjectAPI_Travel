using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectAPI_Travel.Models
{
    public class Bus
    {
        [Key]
        [Required]
        [Column("id_bus")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdBus { get; set; }

        [Column("placa")]
        [StringLength(20)]
        [Required]
        public string Placa { get; set; } = null!;

        [Column("model")]
        [StringLength(50)]
        [Required]
        public string Model { get; set; } = null!;

        [Column("num_columns")]
        [Required]
        public int NumColumns { get; set; }

        [Column("num_rows")]
        [Required]
        public int NumRows { get; set; }

        [Column("availability")]
        [Required]
        public bool Availability { get; set; }

        [Column("state")]
        [Required]
        public bool State { get; set; }

        [InverseProperty("Bus")]
        public virtual ICollection<Seat> Seating { get; set; } = new List<Seat>();

        [InverseProperty("Bus")]
        public virtual ICollection<Itinerary> Itineraries { get; set; } = new List<Itinerary>();
    }
}
