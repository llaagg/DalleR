using DalleR.Models;
namespace DalleR.Controls;

public partial class DashboardView
{
    public override bool HasActions => false;

    private async Task GoToFolders()
    {
        var result = await RunAction<FolderListPayload>("ShowFolders");
        await ParentFrame.Show(result);
    }

    private async Task GoToGalleries()
    {
        var result = await RunAction<GalleryListPayload>("ShowGalleries");
        await ParentFrame.Show(result);
    }

    private async Task GoToSearch()
    {
        var req = new SearchRequest();
        var result = await RunAction<SearchResultPayload>("SearchPictures", req);
        await ParentFrame.Show(result);
    }
}
