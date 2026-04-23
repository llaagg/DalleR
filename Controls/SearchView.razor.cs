using DalleR.Models;
namespace DalleR.Controls;

public partial class SearchView
{
    public override bool HasActions => false;

    private string searchQuery = "";
    private string searchTags = "";
    private double? searchLat;
    private double? searchLon;
    private double? searchRadius;

    protected override void OnInitialized()
    {
        if (Payload?.OriginalRequest != null)
        {
            searchQuery = Payload.OriginalRequest.Query;
            searchTags = Payload.OriginalRequest.Tags;
            searchLat = Payload.OriginalRequest.Latitude;
            searchLon = Payload.OriginalRequest.Longitude;
            searchRadius = Payload.OriginalRequest.RadiusKm;
        }
    }

    private async Task GoToDashboard()
    {
        var result = await RunAction<DashboardPayload>("ShowDashboard");
        await ParentFrame.Show(result);
    }

    private async Task DoSearch()
    {
        var req = new SearchRequest
        {
            Query = searchQuery,
            Tags = searchTags,
            Latitude = searchLat,
            Longitude = searchLon,
            RadiusKm = searchRadius
        };
        var result = await RunAction<SearchResultPayload>("SearchPictures", req);
        await ParentFrame.Show(result);
    }
}
