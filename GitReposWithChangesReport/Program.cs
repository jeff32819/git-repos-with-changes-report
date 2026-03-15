using System.Reflection;
using GitReposWithChangesReport;

var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
var version = assembly.GetName().Version;


Console.Title = $"Git repos with changes - {version}";
Console.WriteLine("");
var appBaseDirectory = AppContext.BaseDirectory;
Console.WriteLine($"Exe located at: {appBaseDirectory}");
Console.WriteLine();

var rootFolders = new List<string>
{
    @"V:\GitHub\Jeff32819",
    @"V:\GitLab"
};



var foldersWithoutGit = new FoldersWithoutGitFolder(rootFolders);
if(foldersWithoutGit.Folders.Count > 0)
{
    Console.WriteLine("Folders without .git folder:");
    Console.ForegroundColor = ConsoleColor.Yellow;
    foreach (var folder in foldersWithoutGit.Folders)
    {
        Console.WriteLine($" - {folder}");
    }
    Console.ResetColor();
    Console.WriteLine();
    Console.WriteLine("-------------------------------------");
    Console.WriteLine("Delete folders without a .git folder? press Y to confirm, any other key to continue");
    var key = Console.ReadKey();
    if (key.Key == ConsoleKey.Y)
    {
        if (foldersWithoutGit.Delete())
        {
            Console.WriteLine("Errors found press any key to continue");
            Console.ReadKey();
        }
    }
}
Console.WriteLine();

Console.WriteLine("Folders being checked for changes:");
foreach (var folder in rootFolders)
{
    Console.WriteLine($" - {folder}");
}

Console.WriteLine();
var gitFolders = GitFolderScanner.FindAllGitFolders(rootFolders);
Console.ForegroundColor = ConsoleColor.Cyan;
Code.AutoContinue("Press any key to continue (auto-continuing in 3 seconds)...", 3);
Console.ResetColor();

Console.WriteLine();
while (true)
{
    RunCode.Process(gitFolders);
}