using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Web.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }  // Hashed password later
    }
}
