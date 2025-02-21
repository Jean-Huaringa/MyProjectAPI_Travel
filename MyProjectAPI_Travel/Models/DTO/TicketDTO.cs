using System.ComponentModel.DataAnnotations;

namespace MyProjectAPI_Travel.Models.DTO
{
    public class TicketDTO
    {
        public int IdUsr { get; set; }

        public int IdWrk { get; set; }

        public int IdItn { get; set; }

        public int IdBus { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public decimal Amount { get; set; }

        public string? UserName { get; set; }

        public string? Lastname { get; set; }

        public string? Age { get; set; }

        public string? TypeDocument { get; set; }

        public string? NumDocument { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
