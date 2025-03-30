using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Web.Models
{
    public class AddStudentViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; }

        public bool Subscribe { get; set; }
    }
}
