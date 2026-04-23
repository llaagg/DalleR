using Revuo.Chat.Abstraction.Base;
namespace DalleR.Models;
public class Gallery : BasePayloadEntity
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> PictureIds { get; set; } = new();
    public DateTime Created { get; set; }
}
