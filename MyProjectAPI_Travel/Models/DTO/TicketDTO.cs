using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyProjectAPI_Travel.Models.DTO
{
    public class TicketDTO
    {
        [Required]
        public int IdUsr { get; set; }

        [Required]
        public int IdWrk { get; set; }

        [Required]
        public int IdItn { get; set; }

        [Required]
        public int IdBus { get; set; }

        [Required]
        public int Row { get; set; }

        [Required]
        public int Column { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = null!;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Lastname { get; set; }

        [Required]
        public string? Age { get; set; }

        [Required]
        public string? TypeDocument { get; set; }

        [Required]
        public string? NumDocument { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
    }
}
