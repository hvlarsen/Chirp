using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;
using Chirp.Infrastructure.Data;
using Chirp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Chirp.Domain.Entities;

namespace Chirp.Razor.Tests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        
        public IntegrationTests(WebApplicationFactory<Program> fixture)
        {

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
            _client = fixture.CreateClient();

        }

        [Fact]
        public async Task CanSeePublicTimeline()
        {
            // Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(connection);

            using var context = new ChirpDbContext(builder.Options);
        await context.Database.MigrateAsync(); // Applies the schema to the database

        // Act
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

        // Assert
            Assert.Contains("Chirp!", content);
            Assert.Contains("Public Timeline", content);
        }

        [Theory]
        [InlineData("alexm")]
        [InlineData("adho")]
        public async Task CanSeePrivateTimeline(string author)
        {
                // Arrange
             using var connection = new SqliteConnection("Filename=:memory:");
            await connection.OpenAsync();
            var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(connection);

            using var context = new ChirpDbContext(builder.Options);
            await context.Database.MigrateAsync(); // Applies the schema to the database

            context.Authors.Add(new Author { Name = "alexm", Email = "a@m.com" });
            context.Authors.Add(new Author { Name = "adho", Email = "a@h.com" });
            await context.SaveChangesAsync();

            var response = await _client.GetAsync($"/{author}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Chirp!", content);
            Assert.Contains($"{author}'s Timeline", content);
        }
    }
    }

