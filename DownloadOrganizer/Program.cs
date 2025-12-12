// to make file organizer
// I need to categorize the type of file that i want to organize
// And need to make those folder with category
// After downloading the files, i have to move these file to the respective oranized folder.
// Need to run the console using windows taks scheduler and cron job

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

class Program
{
     static void Main(string[] args)
    {
        Console.WriteLine($"[{DateTime.Now}] Download Organizer Service starting...");

        CreateHostBuilder(args).Build().Run();
    }
    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<DownloadManagerService>();
            })
            .UseWindowsIfAvailable()
            .UseSystemdIfAvailable();

}




