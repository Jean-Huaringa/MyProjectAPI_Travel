using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectAPI_Travel.Models
{
    public class Ticket
    {
        [Key]
        [Required]
        [Column("id_tck")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTck { get; set; }

        [Column("id_usr")]
        [Required]
        public int IdUsr { get; set; }

        [Column("id_wrk")]
        [Required]
        public int IdWrk { get; set; }

        [Column("id_itn")]
        [Required]
        public int IdItn { get; set; }

        [Column("id_bus")]
        [Required]
        public int IdBus { get; set; }

        [Column("row")]
        [Required]
        public int Row { get; set; }

        [Column("column")]
        [Required]
        public int Column { get; set; }

        [Column("payment_method")]
        [Required]
        public string PaymentMethod { get; set; } = null!;

        [Column("amount", TypeName = "decimal(10, 2)")]
        [Required]
        public decimal Amount { get; set; }

        [Column("username")]
        [Required]
        public string? UserName { get; set; }

        [Column("lastname")]
        [Required]
        public string? Lastname { get; set; }

        [Column("age")]
        [Required]
        public string? Age { get; set; }

        [Column("type_document")]
        [Required]
        public string? TypeDocument { get; set; }

        [Column("num_document")]
        [Required]
        public string? NumDocument { get; set; }

        [Column("state")]
        [Required]
        public bool State { get; set; }

        [Column("problem")]
        [Required]
        public bool? Problem { get; set; }

        [Column("problem_description")]
        public string? ProblemDescription { get; set; }

        [Column("creation_date")]
        [Required]
        public DateTime CreationDate { get; set; }

        [ForeignKey("IdUsr")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("IdWrk")]
        public virtual Worker Worker { get; set; } = null!;

        [ForeignKey("IdItn")]
        public virtual Itinerary Itinerary { get; set; } = null!;

        [ForeignKey("IdBus")]
        public virtual Bus Bus { get; set; } = null!;
    }
}
