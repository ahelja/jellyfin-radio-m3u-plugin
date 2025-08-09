using System.Text;
using MediaBrowser.Controller.Library;
using Microsoft.Extensions.Logging;

namespace RadioM3U;

public class LibraryBuilder
{
    private readonly ILogger _log;
    private readonly ILibraryManager _libraryManager;

    public LibraryBuilder(ILoggerFactory loggerFactory, ILibraryManager libraryManager)
    {
        _log = loggerFactory.CreateLogger<LibraryBuilder>();
        _libraryManager = libraryManager;
    }

    public async Task<int> BuildAsync(string outputPath, IEnumerable<RadioStation> stations, PluginConfiguration cfg)
    {
        Directory.CreateDirectory(outputPath);

        int count = 0;
        foreach (var s in stations)
        {
            var safeName = string.Join("_", s.Name.Split(Path.GetInvalidFileNameChars()));
            var groupName = string.IsNullOrWhiteSpace(s.Group) ? "All Stations" : s.Group!;
            var folder = Path.Combine(outputPath, groupName);
            Directory.CreateDirectory(folder);

            var strmPath = Path.Combine(folder, safeName + ".strm");
            await File.WriteAllTextAsync(strmPath, s.Url + Environment.NewLine, Encoding.UTF8);

            // Try to set station image
            var localImagePath = TryResolveLocalGroupImage(cfg, groupName);
            if (cfg.UseTvgLogos && !string.IsNullOrWhiteSpace(s.LogoUrl))
            {
                var nfo = Path.Combine(folder, safeName + ".nfo");
                var nfoContent = $"<music><thumb>{System.Security.SecurityElement.Escape(s.LogoUrl!)}</thumb></music>";
                await File.WriteAllTextAsync(nfo, nfoContent, Encoding.UTF8);
            }
            else if (!string.IsNullOrWhiteSpace(localImagePath) && File.Exists(localImagePath))
            {
                var dst = Path.Combine(folder, safeName + Path.GetExtension(localImagePath));
                TryCopy(localImagePath!, dst);
            }

            // Ensure folder image for the group
            if (!string.IsNullOrWhiteSpace(localImagePath) && File.Exists(localImagePath))
            {
                var folderJpg = Path.Combine(folder, "folder" + Path.GetExtension(localImagePath));
                if (!File.Exists(folderJpg))
                {
                    TryCopy(localImagePath!, folderJpg);
                }
            }

            count++;
        }
        _log.LogInformation("RadioM3U created {Count} stations at {Path}", count, outputPath);
        return count;
    }

    static string? TryResolveLocalGroupImage(PluginConfiguration cfg, string group)
    {
        if (cfg.GroupDefaultImages.TryGetValue(group, out var path) && !string.IsNullOrWhiteSpace(path))
            return path;
        if (cfg.GroupDefaultImages.TryGetValue("default", out var def) && !string.IsNullOrWhiteSpace(def))
            return def;

        // Also probe an _images folder under output path
        var baseOut = cfg.OutputLibraryPath ?? string.Empty;
        var imagesDir = Path.Combine(baseOut, "_images");
        if (Directory.Exists(imagesDir))
        {
            foreach (var ext in new[] { ".png", ".jpg", ".jpeg" })
            {
                var candidate = Path.Combine(imagesDir, group + ext);
                if (File.Exists(candidate)) return candidate;
            }
            foreach (var ext in new[] { ".png", ".jpg", ".jpeg" })
            {
                var candidate = Path.Combine(imagesDir, "default" + ext);
                if (File.Exists(candidate)) return candidate;
            }
        }
        return null;
    }

    static void TryCopy(string src, string dst)
    {
        try
        {
            File.Copy(src, dst, overwrite: true);
        }
        catch { }
    }
}
