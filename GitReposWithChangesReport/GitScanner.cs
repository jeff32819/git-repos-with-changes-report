using System.Diagnostics;

// ReSharper disable ConvertIfStatementToSwitchStatement

namespace GitReposWithChangesReport;

public static class GitScanner
{
    public static List<GitCheckResult> Scan(string rootFolder)
    {
        var results = new List<GitCheckResult>();

        foreach (var dir in Directory.GetDirectories(rootFolder, "*", SearchOption.AllDirectories))
        {
            var gitFilePath = Path.Combine(dir, ".git");
            if (!Directory.Exists(gitFilePath))
            {
                continue;
            }

            var result = new GitCheckResult
            {
                FilePath = dir,
                HasUnCommitedChanges = HasUncommittedChanges(dir),
                HasUnpushedCommits = HasUnpushedCommits(dir)
            };
            Code.Display(result);
            results.Add(result);
        }

        return results;
    }

    private static bool HasUncommittedChanges(string repoPath)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = "status --porcelain",
            WorkingDirectory = repoPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        var output = process!.StandardOutput.ReadToEnd();
        process.WaitForExit();

        return !string.IsNullOrWhiteSpace(output);
    }

    private static bool HasUnpushedCommits(string repoPath)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = "rev-list --left-right --count @{u}...HEAD",
            WorkingDirectory = repoPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        var output = process!.StandardOutput.ReadToEnd().Trim();
        var error = process.StandardError.ReadToEnd().Trim();
        process.WaitForExit();

        // No upstream branch → treat as no unpushed commits
        if (!string.IsNullOrEmpty(error))
        {
            return false;
        }

        var parts = output.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            return false;
        }

        var behind = int.Parse(parts[0]);
        var ahead = int.Parse(parts[1]);
        return ahead > 0;
    }

    /// <summary>
    ///     good code but do not need to use
    /// </summary>
    public class TextLen_NOT_USING
    {
        public int Length { get; set; }

        public string Add(string txt)
        {
            Length += txt.Length;
            return txt;
        }

        public void WritePadding()
        {
            var padCount = Console.WindowWidth - 1 - Length;
            Console.Write(new string(' ', padCount));
            Console.Write("|");
        }
    }
}