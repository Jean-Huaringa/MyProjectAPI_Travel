using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectAPI_Travel.Models
{
    public class Station
    {
        [Key]
        [Column("id_stn")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdStn { get; set; }

        [Column("city")]
        [StringLength(100)]
        [Required]
        public string? City { get; set; }

        [Column("street")]
        [StringLength(100)]
        [Required]
        public string? Street { get; set; }

        [Column("pseudonym")]
        [StringLength(3)]
        [Required]
        public string? Pseudonym { get; set; }

        [Column("state")]
        [Required]
        public bool State { get; set; }

        public virtual ICollection<Itinerary> Origins { get; set; } = new List<Itinerary>();

        public virtual ICollection<Itinerary> Destinations { get; set; } = new List<Itinerary>();

    }
}
