using DalleR.Models;

namespace DalleR.Services;

public static class SearchEngine
{
    public static IEnumerable<Picture> Filter(IEnumerable<Picture> pictures, SearchRequest req)
    {
        var results = pictures;

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
            var tags = req.Tags
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim().ToLowerInvariant())
                .ToList();
            results = results.Where(p => p.Tags.Any(t => tags.Contains(t.ToLowerInvariant())));
        }

        if (req.Latitude.HasValue && req.Longitude.HasValue && req.RadiusKm.HasValue)
        {
            results = results.Where(p =>
                p.Latitude.HasValue && p.Longitude.HasValue &&
                GetDistance(req.Latitude.Value, req.Longitude.Value, p.Latitude.Value, p.Longitude.Value) <= req.RadiusKm.Value);
        }

        return results;
    }

    public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
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
