using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyProjectAPI_Travel.Models.DTO
{
    public class WorkerDTO
    {
        [Required]
        public int IdWrk { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public string? Role { get; set; }
    }
}
