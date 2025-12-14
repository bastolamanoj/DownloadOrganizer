using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

public class DownloadManagerService : BackgroundService
{
    private FileSystemWatcher _watcher;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = FileOrganizer.Load("Config.json");
        var detector = new DownloadCompletionDetector();

        // a. Start FileSystemWatcher if enabled
        if (config.EnableWatcher)
        {
            _watcher = new FileSystemWatcher(config.DownloadPath)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size,
                Filter = "*.*",
                IncludeSubdirectories = false,
                EnableRaisingEvents = true
            };

            _watcher.Created += async (_, e) =>
            {
                try
                {
                    await detector.WaitUntilReadyAsync(e.FullPath, stoppingToken);
                    config.OrganizeSingleFile(e.FullPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Watcher error: " + ex.Message);
                }
            };
        }


        // 🔹 2. Scheduled scan loop (optional)
        while (!stoppingToken.IsCancellationRequested)
        {
            if (config.EnableScheduledScan)
            {
                try
                {
                    config.Oragnize();
                    Console.WriteLine($"[{DateTime.Now}] Scheduled cleanup done.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Scheduled scan error: " + ex.Message);
                }
            }

            await Task.Delay(
                TimeSpan.FromSeconds(config.ScanIntervalSeconds),
                stoppingToken);
        }
    }


    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _watcher?.Dispose();
        return base.StopAsync(cancellationToken);
    }

}
