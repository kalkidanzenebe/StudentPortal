using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Web.Models.Entities
{
    public class Student
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        public string LearningStyle { get; set; } = string.Empty;
        public string AcademicGoal { get; set; } = string.Empty;
        public string CareerInterest { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Location { get; set; } = string.Empty;
        public string PrimaryLanguage { get; set; } = string.Empty;
        public string PersonalityType { get; set; } = string.Empty;
        public string TechnicalSkills { get; set; } = string.Empty;
        public string Extracurriculars { get; set; } = string.Empty;
        public string AccessedResources { get; set; } = string.Empty;
        [Column(TypeName = "decimal(3,2)")]
        [Range(0.00, 4.00, ErrorMessage = "GPA must be between 0.00 and 4.00")]
        public decimal GPA { get; set; }
        

        public Guid UserId { get; set; }
        public User? User { get; set; }
    }

}