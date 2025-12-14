using System.Reflection.Metadata.Ecma335;
using System.IO;
public class DownloadCompletionDetector
{
    private readonly int _stabilityDelayMs;
    public DownloadCompletionDetector(int stabilityDelayMs=2000)
    {
        _stabilityDelayMs =stabilityDelayMs;
    }

    //Check whether the file is fully downloaded and safe to move
    public bool IsReady(string path)
    {
        if(!File.Exists(path)){
            return false;
        }
        return true;
    }

    //Async wait until the file becomes stable (recommended with FileSystemWatcher)

    public async Task WaitUntilReadyAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (IsTemporaryFile(filePath))
            return;
        long lastSize = -1;

        while (!cancellationToken.IsCancellationRequested)
        {
            if (!File.Exists(filePath))
            return;

            long currentSize = new FileInfo(filePath).Length;

            if (currentSize == lastSize && IsFileUnlocked(filePath))
            return;

            lastSize = currentSize;
            await Task.Delay(_stabilityDelayMs, cancellationToken);
        }
    }

    private bool IsTemporaryFile(string path)
    {
        string ext = Path.GetExtension(path).ToLowerInvariant();
        return ext == ".crdownload" || ext == ".part" || ext == ".download";
    }
    private bool IsFileStable(string path)
    {
        long size1 = new FileInfo(path).Length;
        Thread.Sleep(_stabilityDelayMs);
        long size2 = new FileInfo(path).Length;
        return size1 == size2;
    }

    private bool IsFileUnlocked(string path)
    {
        try
        {
            using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);
            return true;
        }
        catch
        {
            return false;
        }
    }
}