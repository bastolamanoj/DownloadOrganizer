using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using Microsoft.VisualBasic;
public class FileOrganizer
{
    public string DownloadPath { get; set; }
    public bool EnableWatcher { get; set; }
    public bool EnableScheduledScan { get; set; }
    public int ScanIntervalSeconds { get; set; }
    public Dictionary<string, List<string>> Rules { get; set; }
    public FileOrganizer()
    {
        DownloadPath = string.IsNullOrWhiteSpace(DownloadPath)
        ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
        : DownloadPath;
    }
    public static FileOrganizer Load(string configPath)
    {
        var json = File.ReadAllText(configPath);
        return JsonSerializer.Deserialize<FileOrganizer>(json);
    }

    public void Oragnize()
    {
        if (!Directory.Exists(DownloadPath))
            return;

        foreach (var file in Directory.GetFiles(DownloadPath))
        {
            OrganizeSingleFile(file);
        }

    }
    public void OrganizeSingleFile(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        var detector = new DownloadCompletionDetector();

        if (!detector.IsReady(filePath))
            return;

        MoveFileToCategory(filePath);
    }

    // Move file to its category folder based on Rules
    private void MoveFileToCategory(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLowerInvariant();

        foreach (var category in Rules)
        {
            if (category.Value.Contains(extension))
            {
                string categoryFolder = Path.Combine(DownloadPath, category.Key);

                // Create folder if it doesn't exist
                if (!Directory.Exists(categoryFolder))
                    Directory.CreateDirectory(categoryFolder);

                string destFile = Path.Combine(categoryFolder, Path.GetFileName(filePath));

                // Avoid overwrite: add a number if file exists
                int counter = 1;
                string originalDestFile = destFile;
                while (File.Exists(destFile))
                {
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
                    string ext = Path.GetExtension(filePath);
                    destFile = Path.Combine(categoryFolder, $"{fileNameWithoutExt}({counter}){ext}");
                    counter++;
                }

                File.Move(filePath, destFile);
                Console.WriteLine($"Moved {filePath} → {destFile}");
                return;
            }
        }

        // Optional: move to "Others" folder if no match
        string othersFolder = Path.Combine(DownloadPath, "Others");
        if (!Directory.Exists(othersFolder))
            Directory.CreateDirectory(othersFolder);

        string otherDest = Path.Combine(othersFolder, Path.GetFileName(filePath));
        File.Move(filePath, otherDest);
        Console.WriteLine($"Moved {filePath} → {otherDest}");
    }
}