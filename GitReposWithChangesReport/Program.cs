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