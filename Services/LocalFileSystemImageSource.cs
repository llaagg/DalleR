using DalleR.Models;
namespace DalleR.Services;

public class LocalFileSystemImageSource : IImageSource
{
    private static readonly string[] SupportedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tiff", ".tif" };

    public bool IsAvailable => true;
    public string Name => "Local File System";

    public Task<List<Picture>> ScanFolder(string folderPath)
    {
        var pictures = new List<Picture>();
        if (!Directory.Exists(folderPath))
            return Task.FromResult(pictures);

        var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
            .Where(f => SupportedExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()));

        foreach (var file in files)
        {
            try
            {
                var info = new FileInfo(file);
                var picture = new Picture
                {
                    Id = Guid.NewGuid().ToString(),
                    FilePath = file,
                    FileName = info.Name,
                    FolderPath = folderPath,
                    Title = Path.GetFileNameWithoutExtension(file),
                    FileSizeBytes = info.Length,
                    DateScanned = DateTime.UtcNow,
                    DateTaken = info.LastWriteTime
                };
                pictures.Add(picture);
            }
            catch { /* skip files that can't be read */ }
        }

        return Task.FromResult(pictures);
    }
}
