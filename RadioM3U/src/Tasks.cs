using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Plugins;
using Microsoft.Extensions.Logging;

namespace RadioM3U;

public class ImportTask : IServerEntryPoint
{
    private readonly ILogger _log;
    private readonly ILibraryManager _libraryManager;

    public ImportTask(ILoggerFactory loggerFactory, ILibraryManager libraryManager)
    {
        _log = loggerFactory.CreateLogger<ImportTask>();
        _libraryManager = libraryManager;
    }

    public async Task RunAsync()
    {
        await Task.Yield();

        var cfg = Plugin.Instance.Configuration;
        try
        {
            if (string.IsNullOrWhiteSpace(cfg.M3UPath) || !File.Exists(cfg.M3UPath))
            {
                _log.LogWarning("RadioM3U: M3U file not found at {Path}", cfg.M3UPath);
                return;
            }
            var text = await File.ReadAllTextAsync(cfg.M3UPath);
            var stations = M3UParser.Parse(text).ToList();
            var builder = new LibraryBuilder(new LoggerFactory(), _libraryManager);
            var count = await builder.BuildAsync(cfg.OutputLibraryPath, stations, cfg);
            _log.LogInformation("RadioM3U: Imported {Count} stations", count);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "RadioM3U import failed");
        }
    }

    public void Dispose() { }
}
