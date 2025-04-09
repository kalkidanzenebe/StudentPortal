using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models.Entities;
using StudentPortal.Web.Models.ViewModels;
using System.Security.Claims;
[Authorize(Roles = "SuperAdmin")]
public class AdminsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AdminsController(
        ApplicationDbContext context,
        IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    // GET: Admins/AddAdmin
    public IActionResult AddAdmin()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAdmin(AddAdminViewModel model)
    {
        if (ModelState.IsValid)
        {
            var currentUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserIdClaim, out Guid currentUserId))
            {
                ModelState.AddModelError("", "Invalid user session");
                return View(model);
            }

            var admin = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = _passwordHasher.HashPassword(null!, model.Password),
                Role = "Admin",
                CreatedById = currentUserId
            };

            _context.Users.Add(admin);
            await _context.SaveChangesAsync();
            return RedirectToAction("ListAdmin");
        }
        return View(model);
    }

    // GET: Admins/ListAdmin
    public IActionResult ListAdmin()
    {
        var admins = _context.Users
            .Include(u => u.CreatedBy)
            .Where(u => u.Role == "Admin")
            .ToList();

        return View(admins);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var admin = await _context.Users.FirstOrDefaultAsync(u =>
            u.Id == id && u.Role == "Admin");

        if (admin == null)
        {
            TempData["ErrorMessage"] = "Admin not found";
            return RedirectToAction("ListAdmin");
        }

        try
        {
            _context.Users.Remove(admin);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"{admin.Name} deleted successfully";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting admin: {ex.Message}";
        }

        return RedirectToAction("ListAdmin");
    }
}
