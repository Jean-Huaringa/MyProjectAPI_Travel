using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyProjectAPI_Travel.Models.DTO
{
    public class StationDTO
    {
        [Required]
        public string? City { get; set; }

        [Required]
        public string? Street { get; set; }

        [Required]
        public string? Pseudonym { get; set; }
    }
}
