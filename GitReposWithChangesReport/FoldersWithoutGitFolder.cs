namespace GitReposWithChangesReport;

public class FoldersWithoutGitFolder
{
    public FoldersWithoutGitFolder(List<string> parentFolders)
    {
        foreach (var parent in parentFolders)
        {
            if (!Directory.Exists(parent))
            {
                continue;
            }

            // Get all immediate subdirectories for this parent
            var subDirectories = Directory.GetDirectories(parent);

            foreach (var subDir in subDirectories)
            {
                var gitPath = Path.Combine(subDir, ".git");

                if (!Directory.Exists(gitPath))
                {
                    Folders.Add(subDir);
                }
            }
        }
    }

    public List<string> Folders { get; set; } = new();

    public bool Delete()
    {
        var anyErrors = false;
        foreach (var folder in Folders)
        {
            try
            {
                Directory.Delete(folder, true);
                Console.WriteLine($"Deleted: {folder}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to delete {folder}: {ex.Message}");
                Console.ResetColor();
                anyErrors = true;
            }
        }
        return anyErrors;
    }
}