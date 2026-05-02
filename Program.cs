using DrDanielaVintu.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DrDanielaVintu.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Railway/Heroku DATABASE_URL support
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');
    connectionString = $"Host={uri.Host};Port={uri.Port};Username={userInfo[0]};Password={userInfo[1]};Database={uri.AbsolutePath.TrimStart('/')};SSL Mode=Require;Trust Server Certificate=True;";
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Cloudinary Configuration
builder.Services.Configure<DrDanielaVintu.Helpers.CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<DrDanielaVintu.Interfaces.IPhotoService, DrDanielaVintu.Services.PhotoService>();

// Email Configuration
builder.Services.Configure<DrDanielaVintu.Services.EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, DrDanielaVintu.Services.EmailSender>();

var app = builder.Build();

// Seed Admin User
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var context = services.GetRequiredService<ApplicationDbContext>();

    // Apply pending migrations automatically
    await context.Database.MigrateAsync();

    // Roles seeding
    string[] roleNames = { "Admin", "User", "Premium" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Admin user seeding
    var adminEmail = "micleusanudumitru@gmail.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(adminUser, "Daniela999");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
            // Seed Programs (Subscriptions)
    if (!context.SubscriptionPlans.Any())
    {
        context.SubscriptionPlans.AddRange(
            new SubscriptionPlan { 
                Name = "Consultație Unică", 
                Price = 150, 
                IsPopular = false,
                Benefits = "Evaluarea stării de sănătate;Completarea chestionarului de sănătate;Interpretarea analizelor + crearea unei strategii individuale de suplimente;1 consultație online de 60–90 minute"
            },
            new SubscriptionPlan { 
                Name = "Pachet BASIC", 
                Price = 350, 
                IsPopular = false,
                Benefits = "Completarea chestionarului de sănătate;Monitorizare și suport pentru 4 săptămâni;Interpretarea analizelor + crearea unei strategii de suplimente (strict pe baza analizelor);Elaborarea unui plan alimentar personalizat;1 consultație online de 60–90 minute;Urmărirea progresului și rezultatelor;20% REDUCERE la următoarea consultație"
            },
            new SubscriptionPlan { 
                Name = "Pachet VIP", 
                Price = 800, 
                IsPopular = true,
                Benefits = "Completarea chestionarului de sănătate;Monitorizare și suport pentru 10 săptămâni;Interpretarea analizelor + crearea unei strategii de suplimente (strict pe baza analizelor);Elaborarea unui plan alimentar personalizat;4 consultații online de 60–90 minute fiecare;Urmărirea progresului și rezultatelor;20% REDUCERE la următoarea consultație"
            }
        );
        await context.SaveChangesAsync();
    }
}
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
