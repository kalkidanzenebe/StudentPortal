using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;

namespace StudentPortal.Web.Controllers
{
    public class AI : Controller
    {
        private readonly ApplicationDbContext _context;

        public AI(ApplicationDbContext context )
        {
            _context = context;


        }
        public async Task<IActionResult> ExportStudentsToCsv()
        {
            // Get the students data
            var students = await _context.Students
                .Include(s => s.User)
                .OrderBy(s => s.Name)
                .ToListAsync();

            // Create CSV content
            var csv = new StringBuilder();

            // Add header row
            csv.AppendLine("Name,Email,Phone,Age,Location,LearningStyle,AcademicGoal,CareerInterest,GPA");

            // Add data rows
            foreach (var student in students)
            {
                csv.AppendLine($"\"{student.Name}\",\"{student.Email}\",\"{student.Phone}\",{student.Age},\"{student.Location}\",\"{student.LearningStyle}\",\"{student.AcademicGoal}\",\"{student.CareerInterest}\",{student.GPA}");
            }

            // Create the file content
            byte[] fileContents = Encoding.UTF8.GetBytes(csv.ToString());

            // Return the file
            return File(fileContents, "text/csv", "StudentsExport.csv");
        }
    }
}
