using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace StudentPortal.Web.Models.ViewModels
{
    public class AddStudentViewModel
    {
        // Basic Information
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; } = string.Empty;

        // Academic Info
        public string? LearningStyle { get; set; }

        public string? AcademicGoal { get; set; }

        // Career Info
        public string? CareerInterest { get; set; }

        // Demographic Info
        [Range(16, 60, ErrorMessage = "Age must be between 16 and 60.")]
        public int Age { get; set; }

        public string? Location { get; set; }

        public string? PrimaryLanguage { get; set; }

        // Psychometric Info
        public string? PersonalityType { get; set; }

       

        // Multi-select Fields
        [Required(ErrorMessage = "Technical Skills are required.")]
        public List<string> TechnicalSkills { get; set; } = new List<string>();

        [Required(ErrorMessage = "Extracurriculars are required.")]
        public List<string> Extracurriculars { get; set; } = new List<string>();

        [Required(ErrorMessage = "Accessed Resources are required.")]
        public List<string> AccessedResources { get; set; } = new List<string>();

        // Assigned User
        [Required(ErrorMessage = "User assignment is required.")]
        public Guid UserId { get; set; }

        [Display(Name = "GPA")]
        [Range(0.00, 4.00, ErrorMessage = "GPA must be between 0.00 and 4.00")]
        public decimal GPA { get; set; }
        // For dropdown population
        public SelectList? Users { get; set; }
    }
}