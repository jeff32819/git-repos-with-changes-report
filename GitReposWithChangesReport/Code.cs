using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

using static GitReposWithChangesReport.GitScanner;
// ReSharper disable ConvertIfStatementToSwitchStatement
namespace GitReposWithChangesReport
{
    internal class Code
    {
        public static Dictionary<string, string> GetProjectFrameworks(string rootPath)
        {
            var projectMap = new Dictionary<string, string>();
            // Get the files, filter out vshistory, and sort by key (file path)
            var filteredProjects = Directory.EnumerateFiles(rootPath, "*.csproj", SearchOption.AllDirectories)
                .Where(file => !file.Contains("vshistory", StringComparison.OrdinalIgnoreCase))
                .OrderBy(file => file);

            foreach (var file in filteredProjects)
            {
                try
                {
                    var doc = XDocument.Load(file);

                    // Look for TargetFramework OR TargetFrameworks
                    // We use Descendants() to find them regardless of where they are in the XML
                    var tfm = doc.Descendants("TargetFramework").FirstOrDefault()?.Value;
                    var tfms = doc.Descendants("TargetFrameworks").FirstOrDefault()?.Value;

                    // Combine them or take the first one found
                    string result = tfm ?? tfms ?? "Unknown";

                    projectMap[file] = result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not parse {file}: {ex.Message}");
                }
            }

            return projectMap;
        }
        public static ConsoleKeyInfo ReadSingleKey()
        {
            // Flush any extra buffered key presses
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
            // Now read exactly one key
            return Console.ReadKey(true);
        }

        public static void AutoContinue(string message, int seconds)
        {
            Console.WriteLine(message);
            var sw = Stopwatch.StartNew();
            var limit = TimeSpan.FromSeconds(seconds);

            // Loop until a key is pressed OR the time runs out
            while (sw.Elapsed < limit && !Console.KeyAvailable)
            {
                // Optional: Small delay to be kind to the CPU
                Thread.Sleep(100);
            }

            // If a key was pressed, "consume" it so it doesn't leak into the next input
            if (Console.KeyAvailable)
            {
                Console.ReadKey(intercept: true);
            }
        }
        public static void Display(GitCheckResult result)
        {

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{result.FilePath}");
            
            if (result is { HasUnCommitedChanges: false, HasUnpushedCommits: false })
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" - SUCCESS");
                Console.WriteLine();
                Console.ResetColor();
                return;
            }
            if (result.HasUnCommitedChanges)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(" - HAS UNCOMMITED CHANGES");
            }
            if (result.HasUnpushedCommits)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" - HAS UNPUSHED COMMITS");
            }

            Console.WriteLine();
            Console.ResetColor();
            Console.Out.Flush();
        }


        public string ProjectTargetFrameworkReport()
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;
            
            var sb = new StringBuilder();
            sb.AppendLine("<style>");
            sb.AppendLine("  /* This targets all rows in all tables */");
            sb.AppendLine("  tr:hover {");
            sb.AppendLine("    background-color: green;");
            sb.AppendLine("    color: white;");
            sb.AppendLine("    cursor: pointer;");
            sb.AppendLine("  }");
            sb.AppendLine(" body{ font-family:lucida console; } ");
            sb.AppendLine("</style>");
            sb.AppendLine("<table cellpadding=\"3\" border=\"1\">");
            var list = Code.GetProjectFrameworks(@"V:\GitHub\Jeff32819");
            Console.WriteLine($"count = {list.Count}");
            foreach (var item in list)
            {
                sb.AppendLine($"<tr><td>{item.Value}</td><td>{item.Key}</td></tr>");
                Console.WriteLine($"{item.Value,-17} - {item.Key}");
            }
            sb.AppendLine("</table>");
            const string filePath = @"V:\GitHub\project-targetframework-report.html";
            File.WriteAllText(filePath, sb.ToString());
            return filePath;
        }
    }
}
