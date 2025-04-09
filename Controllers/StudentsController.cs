using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models.Entities;
using StudentPortal.Web.Models.ViewModels;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using StudentPortal.Web.Models;
using StudentPortal_Web;
using Microsoft.ML;
using StudentPortal_Web;


namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(
            ApplicationDbContext context,
            ILogger<StudentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            await PopulateUserDropdown();
            return View(new AddStudentViewModel());
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddStudentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await PopulateUserDropdown();
                    return View(model);
                }

                var student = new Student
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name ?? string.Empty,
                    Email = model.Email ?? string.Empty,
                    Phone = model.Phone ?? string.Empty,
                    LearningStyle = model.LearningStyle ?? string.Empty,
                    AcademicGoal = model.AcademicGoal ?? string.Empty,
                    CareerInterest = model.CareerInterest ?? string.Empty,
                    Age = model.Age,
                    Location = model.Location ?? string.Empty,
                    PrimaryLanguage = model.PrimaryLanguage ?? string.Empty,
                    PersonalityType = model.PersonalityType ?? string.Empty,
                   
                    TechnicalSkills = string.Join(",", model.TechnicalSkills),
                    Extracurriculars = string.Join(",", model.Extracurriculars),
                    AccessedResources = string.Join(",", model.AccessedResources),
                    UserId = model.UserId,
                    GPA = model.GPA
                };

                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Student added successfully!";
                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding student");
                TempData["ErrorMessage"] = "Error saving student. Please try again.";
                await PopulateUserDropdown();
                return View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            try
            {
                IQueryable<Student> studentsQuery = _context.Students
                    .Include(s => s.User)
                    .AsNoTracking();

                if (!User.IsInRole("Admin") && !User.IsInRole("SuperAdmin"))
                {
                    var currentUserId = Guid.Parse(
                        User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString()
                    );
                    studentsQuery = studentsQuery.Where(s => s.UserId == currentUserId);
                }

                var students = await studentsQuery
                    .OrderBy(s => s.Name)
                    .ToListAsync();

                return View(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading student list");
                TempData["ErrorMessage"] = "Error loading students.";
                return View(new List<Student>());
            }
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var student = await _context.Students
                    .Include(s => s.User)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (student == null)
                {
                    TempData["ErrorMessage"] = "Student not found";
                    return RedirectToAction(nameof(List));
                }

                await PopulateUserDropdown();

                return View(new EditStudentViewModel
                {
                    Id = student.Id,
                    Name = student.Name ?? string.Empty,
                    Email = student.Email ?? string.Empty,
                    Phone = student.Phone ?? string.Empty,
                    LearningStyle = student.LearningStyle ?? string.Empty,
                    AcademicGoal = student.AcademicGoal ?? string.Empty,
                    CareerInterest = student.CareerInterest ?? string.Empty,
                    Age = student.Age,
                    Location = student.Location ?? string.Empty,
                    PrimaryLanguage = student.PrimaryLanguage ?? string.Empty,
                    PersonalityType = student.PersonalityType ?? string.Empty,
                    UserId = student.UserId,
                    TechnicalSkills = student.TechnicalSkills?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(),
                    Extracurriculars = student.Extracurriculars?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(),
                    AccessedResources = student.AccessedResources?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(),
                    GPA = student.GPA
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading student for edit");
                TempData["ErrorMessage"] = "Error loading student details.";
                return RedirectToAction(nameof(List));
            }
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditStudentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await PopulateUserDropdown();
                    return View(model);
                }

                var student = await _context.Students.FindAsync(model.Id);
                if (student == null)
                {
                    TempData["ErrorMessage"] = "Student not found";
                    return RedirectToAction(nameof(List));
                }

                student.Name = model.Name ?? string.Empty;
                student.Email = model.Email ?? string.Empty;
                student.Phone = model.Phone ?? string.Empty;
                student.LearningStyle = model.LearningStyle ?? string.Empty;
                student.AcademicGoal = model.AcademicGoal ?? string.Empty;
                student.CareerInterest = model.CareerInterest ?? string.Empty;
                student.Age = model.Age;
                student.Location = model.Location ?? string.Empty;
                student.PrimaryLanguage = model.PrimaryLanguage ?? string.Empty;
                student.PersonalityType = model.PersonalityType ?? string.Empty;
                student.UserId = model.UserId;
                student.TechnicalSkills = string.Join(",", model.TechnicalSkills);
                student.Extracurriculars = string.Join(",", model.Extracurriculars);
                student.AccessedResources = string.Join(",", model.AccessedResources);
                student.GPA = model.GPA;

                _context.Students.Update(student);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Student updated successfully!";
                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student");
                TempData["ErrorMessage"] = "Error updating student. Please try again.";
                await PopulateUserDropdown();
                return View(model);
            }
        }
   
private GPAOutputModel CalculateGPA(GPAInputModel input)
    {
        var modelInput = new MLModel.ModelInput
        {
            Age = input.Age,
            LearningStyle = input.LearningStyle,
            AcademicGoal = input.AcademicGoal,
            CareerInterest = input.CareerInterest,
            TechnicalSkills = string.Join(",", input.TechnicalSkills ?? new List<string>()),
            Extracurriculars = string.Join(",", input.Extracurriculars ?? new List<string>()),
            // Required fields with defaults
            Name = "N/A",
            Email = "N/A",
            Phone = "N/A",
            Location = "N/A",
            PrimaryLanguage = "English",
            PersonalityType = "N/A",
            AccessedResources = "N/A",
            Id = 0,
            UserId = 0,
            GPA = 0 // placeholder
        };

        var prediction = MLModel.Predict(modelInput);
        return new GPAOutputModel
        {
            PredictedGPA = (float)Math.Round(prediction.Score, 2),
            Confidence = 0.8f
        };
    }

        // Add this endpoint to your controller
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("PredictGPA")]
        public IActionResult PredictGPA([FromBody] GPAInputModel input)
        {
            try
            {
                if (input == null || input.Age <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Please provide valid student information",
                        predictedGPA = 3.0f
                    });
                }

                // Create minimal valid input
                var modelInput = new MLModel.ModelInput
                {
                    Age = input.Age,
                    LearningStyle = input.LearningStyle ?? "Visual",
                    AcademicGoal = input.AcademicGoal ?? "Bachelor's Degree",
                    CareerInterest = input.CareerInterest ?? "Software Development",
                    TechnicalSkills = string.Join(",", input.TechnicalSkills ?? new List<string>()),
                    Extracurriculars = string.Join(",", input.Extracurriculars ?? new List<string>()),
                    // Required fields with defaults
                    Name = "PREDICTION",
                    Email = "noreply@studentportal.com",
                    Phone = "000-000-0000",
                    Location = "Unknown",
                    PrimaryLanguage = "English",
                    PersonalityType = "INTJ",
                    AccessedResources = "None",
                    Id = 0,
                    UserId = 0,
                    GPA = 0f // placeholder
                };

                try
                {
                    var prediction = MLModel.Predict(modelInput);
                    var predictedGPA = Math.Clamp(prediction.Score, 0f, 4f);

                    return Ok(new
                    {
                        predictedGPA = predictedGPA,
                        message = $"Predicted GPA: {predictedGPA:0.00}"
                    });
                }
                catch (Exception modelEx)
                {
                    _logger.LogError(modelEx, "ML Model Error");
                    return Ok(new
                    {
                        predictedGPA = 3.5f,
                        message = "Using default prediction"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GPA Prediction Error");
                return StatusCode(500, new
                {
                    predictedGPA = 3.0f,
                    message = "Error predicting GPA - Using default value"
                });
            }
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    TempData["ErrorMessage"] = "Student not found";
                    return RedirectToAction(nameof(List));
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Student deleted successfully!";
                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student");
                TempData["ErrorMessage"] = "Error deleting student. Please try again.";
                return RedirectToAction(nameof(List));
            }
        }

        private async Task PopulateUserDropdown()
        {
            try
            {
                var users = await _context.Users
                    .Where(u => u.Role == "User")
                    .OrderBy(u => u.Name)
                    .Select(u => new { u.Id, u.Name })
                    .ToListAsync();

                ViewBag.Users = new SelectList(users, "Id", "Name")
                    ?? new SelectList(Enumerable.Empty<SelectListItem>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading users dropdown");
                ViewBag.Users = new SelectList(Enumerable.Empty<SelectListItem>());
            }
        }
        public class GPAInputModel
        {
            public int Age { get; set; }
            public string? LearningStyle { get; set; }
            public string? AcademicGoal { get; set; }
            public string? CareerInterest { get; set; }
            public List<string>? TechnicalSkills { get; set; }
            public List<string>? Extracurriculars { get; set; }
        }

    }
}