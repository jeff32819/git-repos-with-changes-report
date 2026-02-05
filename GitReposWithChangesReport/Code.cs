using static GitReposWithChangesReport.GitScanner;
// ReSharper disable ConvertIfStatementToSwitchStatement
namespace GitReposWithChangesReport
{
    internal class Code
    {
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
