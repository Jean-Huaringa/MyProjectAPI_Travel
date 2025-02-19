using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectAPI_Travel.Models.DTO
{
    public class ItineraryDTO
    {
        [Required]
        public int IdOrigin { get; set; }

        [Required]
        public int IdDestination { get; set; }

        [Required]
        public int IdWrk { get; set; }

        [Required]
        public int IdBus { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime ArrivalDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        public bool Availability { get; set; } // disponibilidad, aqui se coloca si aun esta disponible el itinerario
    }
}
