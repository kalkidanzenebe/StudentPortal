﻿using Microsoft.AspNetCore.Mvc;
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
                return View(model);
            }

            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                TempData["ErrorMessage"] = "You are not registered. Please sign up.";
                return RedirectToAction("Register", "Auth");
            }

            // Verify the hashed password
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
            if (result == PasswordVerificationResult.Success)
            {

                HttpContext.Session.SetString("UserEmail", user.Email);
                TempData["SuccessMessage"] = "Login successful! Welcome back.";
                return RedirectToAction("List", "Students");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password");
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var existingUser = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email already in use.");
                return View(model);
            }


            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = _passwordHasher.HashPassword(null, model.Password)
            };

            try
            {
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while registering the user.");
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}