using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models.Entities;
using StudentPortal.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace StudentPortal.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(ApplicationDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            this.dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill in all required fields.";
                return View(model);
            }

            // Check if user exists in the database
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user != null && _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password) == PasswordVerificationResult.Success)
            {
                // Store user email in session
                HttpContext.Session.SetString("UserEmail", user.Email);

                // Redirect to Students List page after login
                return RedirectToAction("List", "Students");
            }

            // If login fails
            ModelState.AddModelError(string.Empty, "Invalid email or password");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists in the database
                var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Email is already taken.");
                    return View(model);
                }

                // Create a new user
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password // Ensure you hash passwords in production
                };

                // Save user to the database
                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();

                // Log the user in by storing the email in the session
                HttpContext.Session.SetString("UserEmail", user.Email);

                // Redirect to the Student list
                return RedirectToAction("List", "Students");
            }

            // If we reach here, something went wrong (invalid model)
            return View(model);
        }

    }
}
