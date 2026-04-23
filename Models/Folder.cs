using Revuo.Chat.Abstraction.Base;
namespace DalleR.Models;
public class Folder : BasePayloadEntity
{
    public string Path { get; set; } = "";
    public DateTime? LastScanned { get; set; }
    public bool IsScanning { get; set; }
    public int PictureCount { get; set; }
}
