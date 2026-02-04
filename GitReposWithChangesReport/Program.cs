using System.Diagnostics;
using GitReposWithChangesReport;

Console.WriteLine("");
Console.WriteLine("Place this exe in folder ABOVE your repo folder and it will scan every subfolder for changes that have not been commited/pushed.");
Console.WriteLine("");
var appBaseDirectory = AppContext.BaseDirectory;
Console.WriteLine($"App Base Path = {appBaseDirectory}");
if (Debugger.IsAttached)
{
    Console.WriteLine("Debugger attached, running debug path");
    appBaseDirectory = @"V:\GitHub";
}

Console.WriteLine();
Console.WriteLine($"Scanning base folder: {appBaseDirectory}");
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine($"This may take a few monents");
Console.ResetColor();
Console.WriteLine();
Console.WriteLine("---------------------------------------------------------------------");
Console.WriteLine();
var results = GitScanner.Scan(appBaseDirectory);
foreach (var repo in results)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write($"{repo.FilePath}");
    if (repo.HasUnCommitedChanges)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" - HAS UNCOMMITED CHANGES");
    }
    if (repo.HasUnpushedCommits)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" - HAS UNPUSHED COMMITS");
    }
    Console.WriteLine();
    Console.ResetColor();
}

Console.ResetColor();
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
Console.WriteLine("");
Console.WriteLine("done, press any key to exit");
Console.WriteLine("");
Console.ReadKey();