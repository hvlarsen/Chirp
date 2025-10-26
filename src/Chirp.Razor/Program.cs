using Chirp.Application.Interfaces;
using Chirp.Domain.Entities;
using Chirp.Infrastructure.Data;
using Chirp.Infrastructure.Repositories;
using Chirp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<CheepRepository>();
builder.Services.AddScoped<ICheepService, CheepService>();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ChirpDbContext>(options => options.UseSqlite(connectionString ?? "Data Source=Chirp.db"));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ChirpDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ChirpDbContext>();
    if(!builder.Environment.IsEnvironment("Testing")) //If testing, dont seed data and dont add migrations
    // We manually "seed" data in tests for now
    {
        context.Database.EnsureDeleted();
        context.Database.Migrate(); 
        DbInitializer.SeedDatabase(context);
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

/* using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChirpDbContext>();

    Console.WriteLine("EF Core connected successfully!");
    Console.WriteLine($"Authors table count: {db.Authors.Count()}");
    Console.WriteLine($"Cheeps table count: {db.Cheeps.Count()}");
} */ //Test to check EF Core connection, keep for now, ill remove myself later when needed

app.UseRouting();

app.MapRazorPages();

app.Run();

public partial class Program { } // Needed for WebApplicationFactory<T>
