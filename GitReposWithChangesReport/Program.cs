using System.Diagnostics;
using System.Reflection;

using GitReposWithChangesReport;
var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
var version = assembly.GetName().Version;
Console.Title = $"Git repos with changes - {version}";
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
Console.ForegroundColor = ConsoleColor.Cyan;
Code.AutoContinue("Press any key to continue (auto-continuing in 3 seconds)...", 3);
Console.ResetColor();

Console.WriteLine();
while (true)
{
    RunCode.Process(appBaseDirectory);
}