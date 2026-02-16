using System.Diagnostics;

using static GitReposWithChangesReport.GitScanner;
// ReSharper disable ConvertIfStatementToSwitchStatement
namespace GitReposWithChangesReport
{
    internal class Code
    {
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
    }
}
