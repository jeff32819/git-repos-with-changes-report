namespace GitReposWithChangesReport;

internal class RunCode
{
    public static void Process(List<string> gitFolders)
    {
        Console.WriteLine();
        Console.WriteLine($"Scanning {gitFolders.Count} git folders...");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("This may take a few monents");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("---------------------------------------------------------------------");
        Console.WriteLine();
        Console.CursorVisible = false;
        var results = GitScanner.Scan(gitFolders);
        Console.CursorVisible = true;
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("---------------------------------------------------------------------");
        Console.WriteLine();
        
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("---------------------------------------------------------------------");
        Console.WriteLine();
        var resultsWithIssues = results.Where(x => x.HasUnCommitedChanges || x.HasUnpushedCommits).ToList();
        foreach (var result in resultsWithIssues)
        {
            Code.Display(result);
        }
        Console.WriteLine();
        Console.WriteLine("---------------------------------------------------------------------");
        Console.WriteLine();

        if (resultsWithIssues.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!!! All repos have been commited.");
        }
        else
        {
            var unCommitedCount = resultsWithIssues.Count(x => x.HasUnCommitedChanges);
            var unPushedCount = resultsWithIssues.Count(x => x.HasUnpushedCommits);

            if (unCommitedCount > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{unCommitedCount} repositories have uncommitted changes!");
            }

            if (unPushedCount > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{unPushedCount} repositories have unpushed commits!");
            }
        }


        Console.ResetColor();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Press r for targetframefwork report or any key to run again");
        var key = Code.ReadSingleKey();
        if (key.Key != ConsoleKey.R)
        {
            return;
        }

        var code = new Code();
        var reportFilePath = code.ProjectTargetFrameworkReport();
        Console.WriteLine();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Target framework report saved as:");
        Console.WriteLine();
        Console.WriteLine(reportFilePath);
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Press any key to continue...");
        Console.ResetColor();
        Console.WriteLine();
        Console.ReadKey();
    }
}