using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using StudentPortal.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentPortal")));
builder.Services.AddScoped<IPasswordHasher<User>>(provider =>
    new PasswordHasher<User>());

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.Cookie.HttpOnly = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdmin", policy =>
        policy.RequireRole("SuperAdmin"));
    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"));
});

var app = builder.Build();

// Database seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var hasher = services.GetRequiredService<IPasswordHasher<User>>();

    context.Database.Migrate();

    if (!context.Users.Any(u => u.Role == "SuperAdmin"))
    {
        var superAdmin = new User
        {
            Name = "Kalkidan Zenebe",
            Email = "kalkidanzenebe2552@gmail.com",
            Password = hasher.HashPassword(null!, "kalye@26"),
            Role = "SuperAdmin"
        };
        context.Users.Add(superAdmin);
        context.SaveChanges();
    }
}

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
