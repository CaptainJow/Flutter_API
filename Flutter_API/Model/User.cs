

using Flutter_API.Controllers.Validations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Flutter_API.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        [Required]
        [EmailAddress]
        [EmailUserUnique]
        [Column("email", TypeName = "varchar(254)")]
        public string Email { get; set; }

    }
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
