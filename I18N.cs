using Revuo.Chat.Base.I18N;

namespace DalleR;

public static class I18N
{
    public static TranslationSet set = new TranslationSet()
    {
        Translations =
        {
            ["en-US"] = new Translation()
            {
                Entries =
                {
                    ["DalleR.Dashboard"] = "Dashboard",
                    ["DalleR.Pictures"] = "Pictures",
                    ["DalleR.Folders"] = "Folders",
                    ["DalleR.Galleries"] = "Galleries",
                    ["DalleR.Search"] = "Search",
                    ["DalleR.ManageFolders"] = "Manage Folders",
                    ["DalleR.ManageGalleries"] = "Manage Galleries",
                    ["DalleR.Back"] = "Back",
                    ["DalleR.Add"] = "Add",
                    ["DalleR.AddFolder"] = "Add Folder",
                    ["DalleR.FolderPath"] = "Enter folder path...",
                    ["DalleR.Remove"] = "Remove",
                    ["DalleR.Cancel"] = "Cancel",
                    ["DalleR.Scan"] = "Scan",
                    ["DalleR.Scanning"] = "Scanning...",
                    ["DalleR.LastScanned"] = "Last scanned",
                    ["DalleR.NoFolders"] = "No folders added yet. Add a folder to start scanning for pictures.",
                    ["DalleR.CreateGallery"] = "Create Gallery",
                    ["DalleR.GalleryName"] = "Name",
                    ["DalleR.GalleryDescription"] = "Description",
                    ["DalleR.Create"] = "Create",
                    ["DalleR.Open"] = "Open",
                    ["DalleR.NoGalleries"] = "No galleries yet. Create one to organize your pictures.",
                    ["DalleR.EmptyGallery"] = "This gallery has no pictures yet.",
                    ["DalleR.SearchQuery"] = "Search text",
                    ["DalleR.SearchQueryPlaceholder"] = "Title, filename...",
                    ["DalleR.Tags"] = "Tags",
                    ["DalleR.TagsPlaceholder"] = "tag1, tag2...",
                    ["DalleR.Latitude"] = "Latitude",
                    ["DalleR.Longitude"] = "Longitude",
                    ["DalleR.RadiusKm"] = "Radius (km)",
                    ["DalleR.Results"] = "results",
                    ["DalleR.NoResults"] = "No pictures match your search.",
                    ["DalleR.ShowDashboard"] = "Dashboard",
                    ["DalleR.ShowFolders"] = "Folders",
                    ["DalleR.ShowGalleries"] = "Galleries",
                    ["DalleR.SearchPictures"] = "Search"
                }
            }
        }
    };
}
