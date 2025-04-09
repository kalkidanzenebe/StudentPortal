using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Web.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public string Role { get; set; } = "User";

        public Guid? CreatedById { get; set; }
        public User? CreatedBy { get; set; }
        public List<User> CreatedAdmins { get; set; } = new List<User>();
    }
}