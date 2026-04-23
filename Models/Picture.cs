using Revuo.Chat.Abstraction.Base;
namespace DalleR.Models;
public class Picture : BasePayloadEntity
{
    public string FilePath { get; set; } = "";
    public string FileName { get; set; } = "";
    public string FolderPath { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime? DateTaken { get; set; }
    public DateTime DateScanned { get; set; }
    public long FileSizeBytes { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}
