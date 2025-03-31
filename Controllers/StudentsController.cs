using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;
using StudentPortal.Web.Models.ViewModels;

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
        public IActionResult Add(AddStudentViewModel viewModel)
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

            dbContext.Students.Add(student);
            dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Student added successfully!";
            TempData["HighlightId"] = student.Id;  // Store the Id for highlighting

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult List()
        {
            var students = dbContext.Students.ToList();
            var highlightId = TempData["HighlightId"] as Guid?;
            ViewBag.HighlightId = highlightId;  // Pass HighlightId for the student to highlight
            return View(students);
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
                return RedirectToAction("Login", "Auth");

            var student = dbContext.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student viewModel)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
                return RedirectToAction("Login", "Auth");

            var student = dbContext.Students.Find(viewModel.Id);
            if (student != null)
            {
                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Phone = viewModel.Phone;
                student.Subscribe = viewModel.Subscribe;

                dbContext.SaveChanges();

                TempData["SuccessMessage"] = "Student updated successfully!";
                TempData["HighlightId"] = student.Id;  // Store the Id for highlighting
            }

            return RedirectToAction("List");
        }


        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var student = dbContext.Students.Find(id);
                if (student != null)
                {
                    dbContext.Students.Remove(student);
                    dbContext.SaveChanges();
                    return Json(new { success = true, message = "Student deleted successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Student not found." });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while deleting the student." });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                var student = dbContext.Students.Find(id);
                if (student != null)
                {
                    dbContext.Students.Remove(student);
                    dbContext.SaveChanges();
                    return Json(new { success = true, message = "Student deleted successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Student not found." });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while deleting the student." });
            }
        }

    }
}
