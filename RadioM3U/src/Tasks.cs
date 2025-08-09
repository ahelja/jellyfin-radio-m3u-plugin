using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Plugins;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace RadioM3U;

/// <summary>
/// Task that runs at startup to import radio stations from M3U files
/// </summary>
public class ImportTask : IScheduledTask
{
    private readonly ILogger _log;
    private readonly ILibraryManager _libraryManager;

    public ImportTask(ILoggerFactory loggerFactory, ILibraryManager libraryManager)
    {
        _log = loggerFactory.CreateLogger<ImportTask>();
        _libraryManager = libraryManager;
    }

    public string Name => "Import Radio M3U Stations";
    public string Key => "RadioM3UImportTask";
    public string Description => "Imports radio stations from M3U files";
    public string Category => "RadioM3U";

    public Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
    {
        progress?.Report(0);
        return ExecuteInternal(cancellationToken, progress);
    }

    private async Task ExecuteInternal(CancellationToken cancellationToken, IProgress<double> progress)
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
            progress?.Report(100);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "RadioM3U import failed");
        }
    }

    public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
    {
        // Run at startup and at 2 AM every day
        return new[] {
            new TaskTriggerInfo { Type = TaskTriggerInfo.TriggerStartup },
            new TaskTriggerInfo { Type = TaskTriggerInfo.TriggerDaily, TimeOfDayTicks = TimeSpan.FromHours(2).Ticks }
        };
    }
}
