using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models.Entities;
using StudentPortal.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace StudentPortal.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(
            ApplicationDbContext context,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || _passwordHasher.VerifyHashedPassword(
                user, user.Password, model.Password) == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(model);
            }

            await SignInUser(user);

            return user.Role switch
            {
                "SuperAdmin" => RedirectToAction("SuperAdminDashboard", "Home"),
                "Admin" => RedirectToAction("AdminDashboard", "Home"),
                _ => RedirectToAction("UserDashboard", "Home")
            };
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already registered");
                return View(model);
            }

            // Hash password first
            var hashedPassword = _passwordHasher.HashPassword(null!, model.Password);

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = hashedPassword, // Set in initializer
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            await SignInUser(user);

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult RegisterAdmin() => View();

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdmin(AddAdminViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var currentUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserIdClaim, out Guid currentUserId))
            {
                ModelState.AddModelError("", "Invalid user session");
                return View(model);
            }

            // Hash password first
            var hashedPassword = _passwordHasher.HashPassword(null!, model.Password);

            var admin = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = hashedPassword, // Set in initializer
                Role = "Admin",
                CreatedById = currentUserId
            };

            _context.Users.Add(admin);
            await _context.SaveChangesAsync();
            return RedirectToAction("ListAdmin", "Admins");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties { IsPersistent = true });
        }
    }
}