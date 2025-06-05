using System;
using System.IO;
using System.Text;
using LibrarySystem.Infrastructure;
using LibrarySystem.Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ------------------------
// 1. Настраиваем DbContext на SQLite
// ------------------------
var dbFilePath = Path.Combine(builder.Environment.ContentRootPath, "LibrarySystem.db");
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite($"Data Source={dbFilePath}"));

// ------------------------
// 2. Настраиваем Identity (ApplicationUser + IdentityRole → тот же SQLite)
// ------------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<LibraryDbContext>()
    .AddDefaultTokenProviders();

// ------------------------
// 3. Настраиваем JWT-аутентификацию
// ------------------------
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
var keyBytes = Encoding.UTF8.GetBytes(jwtSettings.Key!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// ------------------------
// 4. Добавляем авторизацию (по ролям/политикам, если нужно)
// ------------------------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireLibrarianRole", policy =>
        policy.RequireRole("Librarian"));
    options.AddPolicy("RequireUserRole", policy =>
        policy.RequireRole("User"));
});

// ------------------------
// 5. Добавляем контроллеры и Swagger
// ------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ------------------------
// 6. Вместо миграций – делаем EnsureCreated()
// ------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Создадим базу и схему, если файл ещё не был создан
    var dbContext = services.GetRequiredService<LibraryDbContext>();
    dbContext.Database.EnsureCreated();

    // ------------------------
    // 6.1. Си́дим роли: Admin, Librarian, User
    // ------------------------
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = new[] { "Admin", "Librarian", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // ------------------------
    // 6.2. Си́дим начального Admin-пользователя
    // ------------------------
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    string adminUserName = "admin";
    string adminEmail = "admin@library.local";
    string adminPassword = "Admin123!";

    if (await userManager.FindByNameAsync(adminUserName) == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = adminUserName,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var createAdminResult = await userManager.CreateAsync(adminUser, adminPassword);
        if (createAdminResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ------------------------
// 7. Включаем аутентификацию и авторизацию
// ------------------------
app.UseAuthentication();
app.UseAuthorization();

// ------------------------
// 8. Мапим контроллеры
// ------------------------
app.MapControllers();

app.Run();


// ========================
// Класс для привязки JwtSettings из appsettings.json
// ========================
public class JwtSettings
{
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? Key { get; set; }
    public int ExpiresMinutes { get; set; }
}
