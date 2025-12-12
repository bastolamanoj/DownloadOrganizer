using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

public class DownloadManagerService : BackgroundService
{
     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
             try
            {
            var config = FileOrganizer.Load("Config.json");
            config.Oragnize();
            Console.WriteLine($"[{DateTime.Now}] Downloads cleaned successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}