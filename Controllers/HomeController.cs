using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using System.Security.Claims;
using System.Linq;

namespace StudentPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index() => View();

        [AllowAnonymous]
        public IActionResult AccessDenied() => View();

        [Authorize]
        public IActionResult Dashboard()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return View(role switch
            {
                "SuperAdmin" => "SuperAdminDashboard",
                "Admin" => "AdminDashboard",
                _ => "UserDashboard"
            });
        }

        [Authorize(Roles = "User")]
        public IActionResult UserDashboard() // No async needed
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            ViewBag.TotalStudents = await _context.Students.CountAsync();
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> SuperAdminDashboard()
        {
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalAdmins = await _context.Users
                .Where(u => u.Role == "Admin")
                .CountAsync();
            return View();
        }
    }
}