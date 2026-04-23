using DalleR.Models;
using DalleR.Helpers;
namespace DalleR.Controls;

public partial class GalleryView
{
    public override bool HasActions => false;

    private static string FormatSize(long bytes) => DisplayHelper.FormatSize(bytes);

    private async Task GoToGalleries()
    {
        var result = await RunAction<GalleryListPayload>("ShowGalleries");
        await ParentFrame.Show(result);
    }

    private async Task RemovePicture(string pictureId)
    {
        var galleryId = Payload?.D?.Id ?? "";
        var req = new GalleryPictureRequest { GalleryId = galleryId, PictureId = pictureId };
        var result = await RunAction<GalleryPayload>("RemovePictureFromGallery", req);
        await ParentFrame.Show(result);
    }
}
