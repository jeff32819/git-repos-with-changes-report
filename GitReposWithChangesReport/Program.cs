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
while (true)
{
    RunCode.Process(appBaseDirectory);
}