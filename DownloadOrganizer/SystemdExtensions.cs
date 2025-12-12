/// <summary>
/// Enable systemd hosting only on Linux
/// </summary>
/// 
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;
static class SystemdExtensions
{
    public static IHostBuilder UseSystemdIfAvailable(this IHostBuilder builder)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            builder.UseSystemd();

        return builder;
    }
}