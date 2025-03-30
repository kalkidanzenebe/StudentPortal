using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StudentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
                return RedirectToAction("Login", "Auth");

            var student = new Student
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Subscribe = viewModel.Subscribe
            };

            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Student added successfully!";
            TempData["MessageType"] = "success";
            return RedirectToAction("List", new { highlightId = student.Id });
        }

        [HttpGet]
        public async Task<IActionResult> List(Guid? highlightId = null)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
                return RedirectToAction("Login", "Auth");

            var students = await dbContext.Students.ToListAsync();
            ViewBag.HighlightId = highlightId; // Pass the ID to the view

            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
                return RedirectToAction("Login", "Auth");

            var student = await dbContext.Students.FindAsync(id);
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student viewModel)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
                return RedirectToAction("Login", "Auth");

            var student = await dbContext.Students.FindAsync(viewModel.Id);
            if (student is not null)
            {
                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Phone = viewModel.Phone;
                student.Subscribe = viewModel.Subscribe;

                await dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Student updated successfully!";
                TempData["MessageType"] = "success"; // Add this line for success message type

            }
            return RedirectToAction("List", new { highlightId = student.Id });
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Delete(Student viewModel)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
                return RedirectToAction("Login", "Auth");

            var student = await dbContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == viewModel.Id);
            if (student is not null)
            {
                dbContext.Students.Remove(student);
                await dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student deleted successfully!";
                TempData["MessageType"] = "delete"; // Add this line for success message type
                                                    // Add this line
            }
            return RedirectToAction("List");
        }

    }
}
