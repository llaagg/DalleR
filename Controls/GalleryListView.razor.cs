using DalleR.Models;
namespace DalleR.Controls;

public partial class GalleryListView
{
    public override bool HasActions => false;

    private bool creatingGallery = false;
    private string newGalleryName = "";
    private string newGalleryDescription = "";

    private async Task GoToDashboard()
    {
        var result = await RunAction<DashboardPayload>("ShowDashboard");
        await ParentFrame.Show(result);
    }

    private async Task ConfirmCreateGallery()
    {
        if (string.IsNullOrWhiteSpace(newGalleryName)) return;
        var req = new CreateGalleryRequest { Name = newGalleryName, Description = newGalleryDescription };
        var result = await RunAction<GalleryListPayload>("CreateGallery", req);
        newGalleryName = "";
        newGalleryDescription = "";
        creatingGallery = false;
        await ParentFrame.Show(result);
    }

    private async Task OpenGallery(string galleryId)
    {
        var req = new GalleryIdRequest { GalleryId = galleryId };
        var result = await RunAction<GalleryPayload>("ShowGallery", req);
        await ParentFrame.Show(result);
    }

    private async Task DeleteGallery(string galleryId)
    {
        var req = new GalleryIdRequest { GalleryId = galleryId };
        var result = await RunAction<GalleryListPayload>("DeleteGallery", req);
        await ParentFrame.Show(result);
    }
}
