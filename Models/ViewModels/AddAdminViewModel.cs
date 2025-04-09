using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Web.Models.ViewModels
{
    public class AddAdminViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }
    }
}