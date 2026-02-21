namespace GitReposWithChangesReport
{
    public static class GitFolderScanner
    {
        public static List<string> FindAllGitFolders(IEnumerable<string> roots)
        {
            var results = new List<string>();

            foreach (var root in roots)
            {
                if (string.IsNullOrWhiteSpace(root) || !Directory.Exists(root))
                {
                    continue;
                }
                try
                {
                    results.AddRange(Directory.EnumerateDirectories(root, ".git", SearchOption.AllDirectories));
                }
                catch (UnauthorizedAccessException)
                {
                    // Skip folders we can't read
                }
                catch (PathTooLongException)
                {
                    // Skip problematic paths
                }
            }
            return results;
        }
    }
}