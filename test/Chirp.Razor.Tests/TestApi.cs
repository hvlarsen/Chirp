using Microsoft.AspNetCore.Mvc.Testing;

namespace Chirp.Razor.Tests;

public class TestAPI : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _fixture;
    private readonly HttpClient _client;

    public TestAPI(WebApplicationFactory<Program> fixture)
    {
        // Copy csv test file 
        var baseDir = AppContext.BaseDirectory;
        var targetPath = Path.Combine(baseDir, "chirp.db");
        var sourcePath = Path.Combine("..", "..", "..", "..", "..", "src", "Chirp.Razor", "chirp.db");

        File.Copy(sourcePath, targetPath, overwrite: true);

        Environment.SetEnvironmentVariable("CHIRPDBPATH", targetPath);

        
        _fixture = fixture;
        _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
    }

    [Fact]
    public async Task CanSeePublicTimeline()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains("Public Timeline", content);
    }

    [Theory]
    [InlineData("alexm")]
    [InlineData("adho")]
    public async Task CanSeePrivateTimeline(string author)
    {
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains($"{author}'s Timeline", content);
    }
}
