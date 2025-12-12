
/// <summary>
/// Enable Windows Service hosting only on Windows
/// </summary>
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;
static class WindowsServiceExtensions
{
    public static IHostBuilder UseWindowsIfAvailable(this IHostBuilder builder)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            builder.UseWindowsService();

        return builder;
    }
}