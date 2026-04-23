using Revuo.Chat.Abstraction.Base;
namespace DalleR.Models;

public class PictureStore : BasePayloadEntity
{
    public static string Key = "dalleR.pictures";
    public List<Picture> Pictures { get; set; } = new();
}

public class FolderStore : BasePayloadEntity
{
    public static string Key = "dalleR.folders";
    public List<Folder> Folders { get; set; } = new();
}

public class GalleryStore : BasePayloadEntity
{
    public static string Key = "dalleR.galleries";
    public List<Gallery> Galleries { get; set; } = new();
}
