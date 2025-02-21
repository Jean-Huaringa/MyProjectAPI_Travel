using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectAPI_Travel.Models
{
    public class Worker
    {
        [Key]
        [Required]
        [Column("id_wrk")]
        public int IdWrk { get; set; }

        [Column("registration_date", TypeName = "datetime")]
        [Required]
        public DateTime? RegistrationDate { get; set; }

        [Column("salary", TypeName = "decimal(10, 2)")]
        public decimal Salary { get; set; }

        [Column("role")]
        [Required]
        public string? Role { get; set; }

        [Column("availability")]
        [Required]
        public bool Availability { get; set; }

        [Column("state")]
        [Required]
        public bool State { get; set; }

        [ForeignKey("IdWrk")]
        public virtual User User { get; set; } = null!;

        [InverseProperty("Worker")]
        public virtual ICollection<Itinerary> Itineraries { get; set; } = new List<Itinerary>();
    }
}
