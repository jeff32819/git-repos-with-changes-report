namespace GitReposWithChangesReport
{
    public class GitCheckResult
    {
        public string FilePath { get; set; } = "";
        public bool HasUnCommitedChanges { get; set; }
        public bool HasUnpushedCommits { get; set; }
    }
}
