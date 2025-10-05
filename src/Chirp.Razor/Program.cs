using Chirp.Application.Interfaces;
using Chirp.Infrastructure.Data;
using Chirp.Infrastructure.Repositories;
using Chirp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// EF Core setup
builder.Services.AddDbContext<ChirpDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddScoped<CheepRepository>();
builder.Services.AddScoped<ICheepService, CheepService>();
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChirpDbContext>();
    Console.WriteLine($"[EF] Database file: {db.Database.GetDbConnection().DataSource}");
    DbInitializer.SeedDatabase(db);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();

public partial class Program { } // Needed for WebApplicationFactory<T>
