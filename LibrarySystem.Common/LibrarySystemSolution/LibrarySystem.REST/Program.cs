using Microsoft.AspNetCore.Builder;          // WebApplication
using Microsoft.Extensions.Hosting;          // IHostEnvironment, app.Environment
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;    // <-- вот это нужно для GetConnectionString
using Microsoft.EntityFrameworkCore;         // для UseSqlServer(...)
using LibrarySystem.Common;                  // ICrudServiceAsync<>
using LibrarySystem.Infrastructure.Data;     // LibraryDbContext
using LibrarySystem.Infrastructure.Services; // CrudServiceAsync<>
using Swashbuckle.AspNetCore;                // AddSwaggerGen, UseSwagger, UseSwaggerUI

var builder = WebApplication.CreateBuilder(args);

// ... остальной код ...
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
// ...


// … остальной код …


// 1) Добавляем контроллеры
builder.Services.AddControllers();

// 2) Swagger (генерация OpenAPI-документации)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3) Регистрируем DbContext (строка подключения берётся из appsettings.json)
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 4) Регистрируем Generic CRUD-сервис
builder.Services.AddTransient(typeof(ICrudServiceAsync<>), typeof(CrudServiceAsync<>));

var app = builder.Build();

// 5) Конфигурируем middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
