using System.Net;
using System.Text;
using Chirp.SimpleDB;
using System.Text.Json;

namespace Chirp.CLI.Client.Tests;

public class ProgramTests
{
    [Fact]
    public async Task CheepCommand_ShouldStoreMessage_AndPrintConfirmation()
    {
        // Arrange
        // Arrange: fake API handler
        var handler = new FakeHttpMessageHandler((req) =>
        {
            if (req.Method == HttpMethod.Post && req.RequestUri!.AbsolutePath == "/cheep")
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        });

        Program.UseHttpClient(new HttpClient(handler) { BaseAddress = new Uri("http://localhost:5165") });

        var args = new[] { "cheep", "Hello world" };
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act
        int exitCode = await Program.Main(args);

        // Assert
        string output = stringWriter.ToString();
        Assert.Equal(0, exitCode);
        Assert.Contains("Cheep added!", output);
    }
    
    [Fact]
    public async Task ReadCommand_ShouldPrintStoredMessages()
    {
        // Arrange
        var cheeps = new List<Cheep>
        {
            new Cheep("tester", "Unit test msg", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        };
        var json = JsonSerializer.Serialize(cheeps);

        var handler = new FakeHttpMessageHandler((req) =>
        {
            if (req.Method == HttpMethod.Get && req.RequestUri!.AbsolutePath == "/cheeps")
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        });

        Program.UseHttpClient(new HttpClient(handler) { BaseAddress = new Uri("http://localhost:5165") });

        var args = new[] { "read" };
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act
        int exitCode = await Program.Main(args);

        // Assert
        string output = stringWriter.ToString();
        Assert.Equal(0, exitCode);
        Assert.Contains("Unit test msg", output);
    }

    [Fact]
    public async Task ReadCommand_ShouldHandleEmptyDatabase()
    {
        // Arrange: fake GET returning an empty list
        var handler = new FakeHttpMessageHandler((req) =>
        {
            if (req.Method == HttpMethod.Get && req.RequestUri!.AbsolutePath == "/cheeps")
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("[]", Encoding.UTF8, "application/json")
                };
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        });

        Program.UseHttpClient(new HttpClient(handler) { BaseAddress = new Uri("http://localhost:5165") });

        var args = new[] { "read" };
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act
        int exitCode = await Program.Main(args);

        // Assert
        string output = stringWriter.ToString();
        Assert.Equal(0, exitCode);
        Assert.True(string.IsNullOrWhiteSpace(output));
    }

    [Fact]
    public async Task CheepCommand_ShouldFail_WhenMessageIsMissing()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler((_) =>
            new HttpResponseMessage(HttpStatusCode.OK)); // not used here
        Program.UseHttpClient(new HttpClient(handler) { BaseAddress = new Uri("http://localhost:5165") });

        var args = new[] { "cheep" }; // missing message argument
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act
        int exitCode = await Program.Main(args);

        // Assert
        string output = stringWriter.ToString();
        Assert.NotEqual(0, exitCode);          // System.CommandLine signals error
        Assert.Contains("Usage:", output);     // help text shown
        Assert.Contains("cheep", output);      // command name appears
    }
}

public class FakeHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responder) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromResult(responder(request));
}


