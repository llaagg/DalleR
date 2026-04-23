using Revuo.Chat.Abstraction.Client;
using Revuo.Chat.Base;
using Revuo.Chat.Client.Base.Abstractions;
using Revuo.Chat.Base.I18N;
using Revuo.Chat.Abstraction;
using Revuo.Chat.Abstraction.Base;
using DalleR.Models;
using DalleR.Services;
using DalleR.Controls;

namespace DalleR;

public class DalleRApp : BaseThinClientApp
{
    private readonly IImageSource _imageSource = new LocalFileSystemImageSource();

    public DalleRApp() : base(new StaticTranslator(I18N.set))
    {
    }

    protected override Task OnInit()
    {
        AddAction(ShowDashboard);
        AddAction(ShowFolders);
        AddAction<AddFolderRequest, FolderListPayload>(AddFolder);
        AddAction<FolderPathRequest, FolderListPayload>(RemoveFolder);
        AddAction<FolderPathRequest, FolderListPayload>(ScanFolder);
        AddAction(RefreshAllFolders);
        AddAction(ShowGalleries);
        AddAction<GalleryIdRequest, GalleryPayload>(ShowGallery);
        AddAction<CreateGalleryRequest, GalleryListPayload>(CreateGallery);
        AddAction<GalleryIdRequest, GalleryListPayload>(DeleteGallery);
        AddAction<GalleryPictureRequest, GalleryPayload>(AddPictureToGallery);
        AddAction<GalleryPictureRequest, GalleryPayload>(RemovePictureFromGallery);
        AddAction<SearchRequest, SearchResultPayload>(SearchPictures);

        AddControl<DashboardView>();
        AddControl<FolderListView>();
        AddControl<GalleryListView>();
        AddControl<GalleryView>();
        AddControl<SearchView>();

        return Task.CompletedTask;
    }

    private async Task<DashboardPayload> ShowDashboard(IThinClientContext ctx)
    {
        var storage = StorageHelper.GetStorage(ctx);
        var pictureStore = await storage.Get<PictureStore>(PictureStore.Key) ?? new PictureStore { Id = PictureStore.Key };
        var folderStore = await storage.Get<FolderStore>(FolderStore.Key) ?? new FolderStore { Id = FolderStore.Key };
        var galleryStore = await storage.Get<GalleryStore>(GalleryStore.Key) ?? new GalleryStore { Id = GalleryStore.Key };

        return new DashboardPayload
        {
            TotalPictures = pictureStore.Pictures.Count,
            TotalFolders = folderStore.Folders.Count,
            TotalGalleries = galleryStore.Galleries.Count
        };
    }

    private async Task<FolderListPayload> ShowFolders(IThinClientContext ctx)
    {
        var storage = StorageHelper.GetStorage(ctx);
        var folderStore = await storage.Get<FolderStore>(FolderStore.Key) ?? new FolderStore { Id = FolderStore.Key };
        return new FolderListPayload { D = folderStore.Folders };
    }

    private async Task<FolderListPayload> AddFolder(IThinClientContext ctx, AddFolderRequest req)
    {
        var storage = StorageHelper.GetStorage(ctx);
        var folderStore = await storage.Get<FolderStore>(FolderStore.Key) ?? new FolderStore { Id = FolderStore.Key };

        if (!folderStore.Folders.Any(f => f.Path == req.FolderPath))
        {
            folderStore.Folders.Add(new Folder
            {
                Id = Guid.NewGuid().ToString(),
                Path = req.FolderPath
            });
            folderStore.Id = FolderStore.Key;
            await storage.Store(folderStore);
        }

        return new FolderListPayload { D = folderStore.Folders };
    }

    private async Task<FolderListPayload> RemoveFolder(IThinClientContext ctx, FolderPathRequest req)
    {
        var storage = StorageHelper.GetStorage(ctx);
        var folderStore = await storage.Get<FolderStore>(FolderStore.Key) ?? new FolderStore { Id = FolderStore.Key };
        folderStore.Folders.RemoveAll(f => f.Path == req.FolderPath);
        folderStore.Id = FolderStore.Key;
        await storage.Store(folderStore);
        return new FolderListPayload { D = folderStore.Folders };
    }

    private async Task<FolderListPayload> ScanFolder(IThinClientContext ctx, FolderPathRequest req)
    {
        var storage = StorageHelper.GetStorage(ctx);
        var folderStore = await storage.Get<FolderStore>(FolderStore.Key) ?? new FolderStore { Id = FolderStore.Key };
        var pictureStore = await storage.Get<PictureStore>(PictureStore.Key) ?? new PictureStore { Id = PictureStore.Key };

        var folder = folderStore.Folders.FirstOrDefault(f => f.Path == req.FolderPath);
        if (folder != null)
        {
            folder.IsScanning = true;
            var scanned = await _imageSource.ScanFolder(req.FolderPath);
            pictureStore.Pictures.RemoveAll(p => p.FolderPath == req.FolderPath);
            pictureStore.Pictures.AddRange(scanned);
            folder.PictureCount = scanned.Count;
            folder.LastScanned = DateTime.UtcNow;
            folder.IsScanning = false;

            pictureStore.Id = PictureStore.Key;
            folderStore.Id = FolderStore.Key;
            await storage.Store(pictureStore);
            await storage.Store(folderStore);
        }

        return new FolderListPayload { D = folderStore.Folders };
    }

    private async Task<FolderListPayload> RefreshAllFolders(IThinClientContext ctx)
    {
        var storage = StorageHelper.GetStorage(ctx);
        var folderStore = await storage.Get<FolderStore>(FolderStore.Key) ?? new FolderStore { Id = FolderStore.Key };
        var pictureStore = await storage.Get<PictureStore>(PictureStore.Key) ?? new PictureStore { Id = PictureStore.Key };

        foreach (var folder in folderStore.Folders)
        {
            var scanned = await _imageSource.ScanFolder(folder.Path);
            pictureStore.Pictures.RemoveAll(p => p.FolderPath == folder.Path);
            pictureStore.Pictures.AddRange(scanned);
            folder.PictureCount = scanned.Count;
            folder.LastScanned = DateTime.UtcNow;
        }

        pictureStore.Id = PictureStore.Key;
        folderStore.Id = FolderStore.Key;
        await storage.Store(pictureStore);
        await storage.Store(folderStore);

        return new FolderListPayload { D = folderStore.Folders };
    }

    private async Task<GalleryListPayload> ShowGalleries(IThinClientContext ctx)
    {
        var storage = StorageHelper.GetStorage(ctx);
        var galleryStore = await storage.Get<GalleryStore>(GalleryStore.Key) ?? new GalleryStore { Id = GalleryStore.Key };
        return new GalleryListPayload { D = galleryStore.Galleries };
    }

    private async Task<GalleryPayload> ShowGallery(IThinClientContext ctx, GalleryIdRequest req)
    {
        var storage = StorageHelper.GetStorage(ctx);
        var galleryStore = await storage.Get<GalleryStore>(GalleryStore.Key) ?? new GalleryStore { Id = GalleryStore.Key };
        var pictureStore = await storage.Get<PictureStore>(PictureStore.Key) ?? new PictureStore { Id = PictureStore.Key };

        var gallery = galleryStore.Galleries.FirstOrDefault(g => g.Id == req.GalleryId) ?? new Gallery();
        var pictures = pictureStore.Pictures.Where(p => gallery.PictureIds.Contains(p.Id ?? "")).ToList();

        return new GalleryPayload { D = gallery, Pictures = pictures };
    }

    private async Task<GalleryListPayload> CreateGallery(IThinClientContext ctx, CreateGalleryRequest req)
    {
        var storage = StorageHelper.GetStorage(ctx);
        var galleryStore = await storage.Get<GalleryStore>(GalleryStore.Key) ?? new GalleryStore { Id = GalleryStore.Key };

        galleryStore.Galleries.Add(new Gallery
        {
            Id = Guid.NewGuid().ToString(),
            Name = req.Name,
            Description = req.Description,
            Created = DateTime.UtcNow
        });
        galleryStore.Id = GalleryStore.Key;
        await storage.Store(galleryStore);

        return new GalleryListPayload { D = galleryStore.Galleries };
    }

    private async Task<GalleryListPayload> DeleteGallery(IThinClientContext ctx, GalleryIdRequest req)
    {
        var storage = StorageHelper.GetStorage(ctx);
        var galleryStore = await storage.Get<GalleryStore>(GalleryStore.Key) ?? new GalleryStore { Id = GalleryStore.Key };
        galleryStore.Galleries.RemoveAll(g => g.Id == req.GalleryId);
        galleryStore.Id = GalleryStore.Key;
        await storage.Store(galleryStore);
        return new GalleryListPayload { D = galleryStore.Galleries };
    }

    private async Task<GalleryPayload> AddPictureToGallery(IThinClientContext ctx, GalleryPictureRequest req)
    {
        var storage = StorageHelper.GetStorage(ctx);
        var galleryStore = await storage.Get<GalleryStore>(GalleryStore.Key) ?? new GalleryStore { Id = GalleryStore.Key };
        var pictureStore = await storage.Get<PictureStore>(PictureStore.Key) ?? new PictureStore { Id = PictureStore.Key };

        var gallery = galleryStore.Galleries.FirstOrDefault(g => g.Id == req.GalleryId);
        if (gallery != null && !gallery.PictureIds.Contains(req.PictureId))
        {
            gallery.PictureIds.Add(req.PictureId);
            galleryStore.Id = GalleryStore.Key;
            await storage.Store(galleryStore);
        }

        var pictures = pictureStore.Pictures.Where(p => gallery?.PictureIds.Contains(p.Id ?? "") == true).ToList();
        return new GalleryPayload { D = gallery, Pictures = pictures };
    }

    private async Task<GalleryPayload> RemovePictureFromGallery(IThinClientContext ctx, GalleryPictureRequest req)
    {
        var storage = StorageHelper.GetStorage(ctx);
        var galleryStore = await storage.Get<GalleryStore>(GalleryStore.Key) ?? new GalleryStore { Id = GalleryStore.Key };
        var pictureStore = await storage.Get<PictureStore>(PictureStore.Key) ?? new PictureStore { Id = PictureStore.Key };

        var gallery = galleryStore.Galleries.FirstOrDefault(g => g.Id == req.GalleryId);
        if (gallery != null)
        {
            gallery.PictureIds.Remove(req.PictureId);
            galleryStore.Id = GalleryStore.Key;
            await storage.Store(galleryStore);
        }

        var pictures = pictureStore.Pictures.Where(p => gallery?.PictureIds.Contains(p.Id ?? "") == true).ToList();
        return new GalleryPayload { D = gallery, Pictures = pictures };
    }

    private async Task<SearchResultPayload> SearchPictures(IThinClientContext ctx, SearchRequest req)
    {
        var storage = StorageHelper.GetStorage(ctx);
        var pictureStore = await storage.Get<PictureStore>(PictureStore.Key) ?? new PictureStore { Id = PictureStore.Key };

        var results = pictureStore.Pictures.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(req.Query))
        {
            var q = req.Query.ToLowerInvariant();
            results = results.Where(p =>
                p.Title.ToLowerInvariant().Contains(q) ||
                p.FileName.ToLowerInvariant().Contains(q) ||
                p.Description.ToLowerInvariant().Contains(q));
        }

        if (!string.IsNullOrWhiteSpace(req.Tags))
        {
            var tags = req.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim().ToLowerInvariant()).ToList();
            results = results.Where(p => p.Tags.Any(t => tags.Contains(t.ToLowerInvariant())));
        }

        if (req.Latitude.HasValue && req.Longitude.HasValue && req.RadiusKm.HasValue)
        {
            results = results.Where(p => p.Latitude.HasValue && p.Longitude.HasValue &&
                GetDistance(req.Latitude.Value, req.Longitude.Value, p.Latitude.Value, p.Longitude.Value) <= req.RadiusKm.Value);
        }

        return new SearchResultPayload { D = results.ToList(), OriginalRequest = req };
    }

    private static double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371;
        var dLat = (lat2 - lat1) * Math.PI / 180;
        var dLon = (lon2 - lon1) * Math.PI / 180;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }
}