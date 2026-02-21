using System.Diagnostics;

// ReSharper disable ConvertIfStatementToSwitchStatement

namespace GitReposWithChangesReport;

public static class GitScanner
{
    public static List<GitCheckResult> Scan(List<string> gitFolders)
    {
        var results = new List<GitCheckResult>();
        var index = 0;
        var totalString = gitFolders.Count.ToString().PadLeft(3);
        foreach (var dir in gitFolders)
        {
            var indexText = index.ToString().PadLeft(3);
            var pct = ((index + 1) * 100.0 / gitFolders.Count);
            var pctText = ((int)Math.Round(pct)).ToString().PadLeft(3);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{indexText} / {totalString} ({pctText}% ) ");
            Console.ResetColor();

            var parentFolder = Directory.GetParent(dir);
            if (parentFolder == null)
            {
                throw new Exception("parentFolder is null, should never happen");
            }
            index++;
            var result = new GitCheckResult
            {
                FilePath = parentFolder.FullName,
                HasUnCommitedChanges = HasUncommittedChanges(parentFolder.FullName),
                HasUnpushedCommits = HasUnpushedCommits(parentFolder.FullName)
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
}