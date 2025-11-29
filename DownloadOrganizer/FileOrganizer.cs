using System.Text.Json;
public class FileOrganizer
{
    public string DonwloadPath {get;set;} 
    public Dicotonary<string, List<string>> Rules{get;set;}

    public static FileOrganizer Load(string configPath)
    {
        var json = File.ReadAllText(configPath);
        return JsonSerilizer.Deserilize<FileOrganizer>(json);
    }

    public void Oragnize()
    {
        var downloads = string.IsNullOrWhiteSpace(DonwloadPath) 
        ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"Downloads") 
        : DonwloadPath;

        foreach(var rule in Rules)
        {
            string categoryFolder = Path.Combine(downloads,rule,key); 
            Directory.CreateDirectory(categoryFolder);

            foreach(var pattern in rule)
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