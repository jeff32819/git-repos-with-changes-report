using System.Diagnostics;

namespace GitReposWithChangesReport
{

    public static class GitScanner
    {
        public static List<GitCheckResult> Scan(string rootFolder)
        {
            var results = new List<GitCheckResult>();

            foreach (var dir in Directory.GetDirectories(rootFolder, "*", SearchOption.AllDirectories))
            {
                if (Directory.Exists(Path.Combine(dir, ".git")))
                {
                    var uncommitted = HasUncommittedChanges(dir);
                    var unpushed = HasUnpushedCommits(dir);

                    if (uncommitted || unpushed)
                    {
                        results.Add(new GitCheckResult
                        {
                            FilePath = dir,
                            HasUnCommitedChanges = uncommitted,
                            HasUnpushedCommits = unpushed
                        });
                    }
                }
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
            string output = process!.StandardOutput.ReadToEnd();
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
            string output = process!.StandardOutput.ReadToEnd().Trim();
            string error = process.StandardError.ReadToEnd().Trim();
            process.WaitForExit();

            // No upstream branch → treat as no unpushed commits
            if (!string.IsNullOrEmpty(error))
                return false;

            var parts = output.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                return false;

            int behind = int.Parse(parts[0]);
            int ahead = int.Parse(parts[1]);

            return ahead > 0;
        }
    }

}
