using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectAPI_Travel.Models
{
    public class User
    {
        [Key]
        [Required]
        [Column("id_usr")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsr { get; set; }

        [Column("username")]
        [StringLength(100)]
        [Required]
        public string? UserName { get; set; }

        [Column("lastname")]
        [StringLength(100)]
        [Required]
        public string? Lastname { get; set; }

        [Column("phone")]
        [StringLength(15)]
        public string? Phone { get; set; }

        [Column("birtdate", TypeName = "date")]
        [Required]
        public DateOnly Birthdate { get; set; }

        [Column("type_document")]
        [StringLength(20)]
        [Required]
        public string? TypeDocument { get; set; }

        [Column("num_document")]
        [StringLength(8)]
        [Required]
        public string? NumDocument { get; set; }

        [Column("mail")]
        [StringLength(100)]
        [Required]
        public string? Mail { get; set; }

        [Column("password")]
        [StringLength(100)]
        [Required]
        public string? Password { get; set; }

        [Column("state")]
        [Required]
        public bool? State { get; set; }

        [Column("ban")]
        [Required]
        public bool? Ban { get; set; }

        [Column("registration_date", TypeName = "datetime")]
        [Required]
        public DateTime RegistrationDate { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        [InverseProperty("User")]
        public virtual Worker? Worker { get; set; }
    }
}
