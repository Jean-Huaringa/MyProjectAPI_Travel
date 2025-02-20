using System.ComponentModel.DataAnnotations;

namespace MyProjectAPI_Travel.Models.DTO
{
    public class UserDTO
    {
        [StringLength(100)]
        [Required]
        public string? UserName { get; set; }

        [StringLength(100)]
        [Required]
        public string? Lastname { get; set; }

        [StringLength(9)]
        public string? Phone { get; set; }

        [Required]
        public DateOnly Birthdate { get; set; }

        [StringLength(20)]
        [Required]
        public string? TypeDocument { get; set; }

        [StringLength(8)]
        [Required]
        public string? NumDocument { get; set; }

        [StringLength(100)]
        [Required]
        public string? Mail { get; set; }

        public string? Password { get; set; }

    }
}
