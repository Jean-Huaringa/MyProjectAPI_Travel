using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyProjectAPI_Travel.Models.DTO
{
    public class BusDTO
    {
        [Required]
        public string Placa { get; set; } = null!;

        [Required]
        public string Model { get; set; } = null!;

        [Required]
        public int NumColumns { get; set; }

        [Required]
        public int NumRows { get; set; }
    }
}
