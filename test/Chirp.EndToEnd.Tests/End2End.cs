using System.Diagnostics;

namespace Chirp.EndToEnd.Test;

public class End2End : IDisposable
{
    private Process? _apiProcess;

    public End2End()
    {
        StartApi();
    }
    
    [Fact]
    public void TestReadCheep()
    {
        // Arrange
        ArrangeTestDatabase();
        string output = RunCli("read");

        // Act
        string fstCheep = output.Split("\n")[0].TrimEnd('\r', '\n');

        // Assert
        Assert.StartsWith("ropf", fstCheep);
        Assert.EndsWith("Hello, BDSA students!", fstCheep);
    }

    [Fact]
    public void TestStoreCheep()
    {
        // Arrange
        ArrangeTestDatabase();
        string testMessage = "This is a test cheep!";

        // Act – store new cheep
        RunCli($"cheep \"{testMessage}\"");

        // Assert – read back and check the last line
        string output = RunCli("read");
        string lastCheep = output.Split("\n")
                                 .Last(line => !string.IsNullOrWhiteSpace(line))
                                 .TrimEnd('\r', '\n');

        Assert.Contains(testMessage, lastCheep);
        Assert.StartsWith(Environment.UserName, lastCheep);
    }
    
    private void StartApi()
    {
        var apiDll = Path.GetFullPath(
            Path.Combine("..", "..", "..", "..", "..", "src", "Chirp.CSVDBService",
                "bin", "Debug", "net8.0", "Chirp.CSVDBService.dll"));

        _apiProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"\"{apiDll}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        _apiProcess.Start();

        // Give API a bit of time to boot
        Thread.Sleep(3000);
    }

    private static string RunCli(string arguments)
    {
        var cliDll = Path.GetFullPath(
            Path.Combine("..", "..", "..", "..", "..", "src", "Chirp.CLI.Client",
                         "bin", "Debug", "net8.0", "Chirp.CLI.Client.dll")
        );

        using var process = new Process();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.Arguments = $"\"{cliDll}\" {arguments}";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output;
    }
    
    private void ArrangeTestDatabase()
    {
        var exeDir = Path.GetFullPath(
            Path.Combine("..", "..", "..", "..", "..", "src", "Chirp.CLI.Client",
                         "bin", "Debug", "net8.0")
        );
        var dbPath = Path.Combine(exeDir, "chirp_cli_db.csv");

        var templatePath = Path.GetFullPath(
            Path.Combine("..", "..", "..", "..", "..", "data", "chirp_cli_db_test.csv")
        );

        File.Copy(templatePath, dbPath, overwrite: true);
    }
    
    public void Dispose()
    {
        if (_apiProcess != null && !_apiProcess.HasExited)
        {
            _apiProcess.Kill(entireProcessTree: true);
            _apiProcess.Dispose();
        }
    }
}