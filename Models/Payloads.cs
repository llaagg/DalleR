using Revuo.Chat.Abstraction.Base;
namespace DalleR.Models;

// Dashboard
public class DashboardPayload : BasePayload
{
    public int TotalPictures { get; set; }
    public int TotalFolders { get; set; }
    public int TotalGalleries { get; set; }
}

// Folders
public class FolderListPayload : BasePayload<List<Folder>> { }

public class AddFolderRequest : BasePayload
{
    public string FolderPath { get; set; } = "";
}

public class FolderPathRequest : BasePayload
{
    public string FolderPath { get; set; } = "";
}

// Galleries
public class GalleryListPayload : BasePayload<List<Gallery>> { }

public class GalleryPayload : BasePayload<Gallery>
{
    public List<Picture> Pictures { get; set; } = new();
}

public class CreateGalleryRequest : BasePayload
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}

public class GalleryIdRequest : BasePayload
{
    public string GalleryId { get; set; } = "";
}

public class GalleryPictureRequest : BasePayload
{
    public string GalleryId { get; set; } = "";
    public string PictureId { get; set; } = "";
}

// Search
public class SearchRequest : BasePayload
{
    public string Query { get; set; } = "";
    public string Tags { get; set; } = "";
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? RadiusKm { get; set; }
}

public class SearchResultPayload : BasePayload<List<Picture>>
{
    public SearchRequest? OriginalRequest { get; set; }
}
