using DalleR.Models;
namespace DalleR.Services;

public interface IImageSource
{
    Task<List<Picture>> ScanFolder(string folderPath);
    bool IsAvailable { get; }
    string Name { get; }
}
