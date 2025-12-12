using System.Text.Json;
public class FileOrganizer
{
    public string DonwloadPath {get;set;} 
    public Dictionary<string, List<string>> Rules{get;set;}

    public static FileOrganizer Load(string configPath)
    {
        var json = File.ReadAllText(configPath);
        return JsonSerializer.Deserialize<FileOrganizer>(json);
    }

    public void Oragnize()
    {
        var downloads = string.IsNullOrWhiteSpace(DonwloadPath) 
        ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"Downloads") 
        : DonwloadPath;

        foreach(var rule in Rules)
        {
            string categoryFolder = Path.Combine(downloads,rule.Key); 
            Directory.CreateDirectory(categoryFolder);

            foreach(var pattern in rule.Value)
            {
                var files = Directory.GetFiles(downloads, pattern, SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    string dest = Path.Combine(categoryFolder, Path.GetFileName(file));
                    File.Move(file, dest, true);
                }
            }
        }
    }
}