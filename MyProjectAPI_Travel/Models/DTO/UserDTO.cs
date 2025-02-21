using System.ComponentModel.DataAnnotations;

namespace MyProjectAPI_Travel.Models.DTO
{
    public class UserDTO
    {
        [StringLength(100)]
        public string? UserName { get; set; }

        [StringLength(100)]
        public string? Lastname { get; set; }

        [StringLength(9)]
        public string? Phone { get; set; }

        public DateOnly Birthdate { get; set; }

        [StringLength(20)]
        public string? TypeDocument { get; set; }

        [StringLength(8)]
        public string? NumDocument { get; set; }

        [StringLength(100)]
        public string? Mail { get; set; }

        public string? Password { get; set; }

    }
}
