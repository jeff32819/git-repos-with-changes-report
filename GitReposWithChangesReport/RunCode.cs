namespace GitReposWithChangesReport;

internal class RunCode
{
    public static void Process(string appBaseDirectory)
    {
        Console.WriteLine();
        Console.WriteLine($"Scanning base folder: {appBaseDirectory}");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("This may take a few monents");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("---------------------------------------------------------------------");
        Console.WriteLine();
        Console.CursorVisible = false;
        var results = GitScanner.Scan(appBaseDirectory);
        Console.CursorVisible = true;
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("---------------------------------------------------------------------");
        Console.WriteLine();
        
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("---------------------------------------------------------------------");
        Console.WriteLine();
        foreach (var result in results.Where(x => x.HasUnCommitedChanges || x.HasUnpushedCommits).ToList())
        {
            Code.Display(result);
        }
        Console.WriteLine();
        Console.WriteLine("---------------------------------------------------------------------");
        Console.WriteLine();

        if (results.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!!! All repos have been commited.");
        }
        else
        {
            var unCommitedCount = results.Count(x => x.HasUnCommitedChanges);
            var unPushedCount = results.Count(x => x.HasUnpushedCommits);

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
        Console.WriteLine("Press any key to run again");
        Console.ResetColor();
        Console.ReadKey();
    }
}