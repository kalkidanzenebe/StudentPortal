using System.Collections.Generic;

namespace StudentPortal.Web.Models
{
    public class GPAInputModel
    {
        public int Age { get; set; }
        public string? LearningStyle { get; set; }
        public string? AcademicGoal { get; set; }
        public string? CareerInterest { get; set; }
        public List<string>? TechnicalSkills { get; set; }
        public List<string>? Extracurriculars { get; set; }
        public string PrimaryLanguage { get; set; } = "English"; // Default value
    }
}