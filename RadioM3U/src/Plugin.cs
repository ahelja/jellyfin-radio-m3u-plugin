using System;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace RadioM3U;

public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    public static Plugin Instance { get; private set; } = null!;
    public override string Name => "RadioM3U";
    public override Guid Id => new ("f5c7a89d-107b-4c2f-9f17-6b6540b8e3e8");

    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, ILoggerFactory loggerFactory)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
        Logger = loggerFactory.CreateLogger<Plugin>();
    }

    public ILogger Logger { get; }

    public override PluginInfo GetPluginInfo() => new()
    {
        Name = Name,
        Id = Id
    };

    public System.Collections.Generic.IEnumerable<MediaBrowser.Model.Plugins.PluginPageInfo> GetPages()
    {
        yield return new MediaBrowser.Model.Plugins.PluginPageInfo
        {
            Name = "configPage",
            EmbeddedResourcePath = GetType().Namespace + ".Web.configPage.html"
        };
    }
}
