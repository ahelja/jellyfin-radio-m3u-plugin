using System.Collections.Generic;
using MediaBrowser.Model.Plugins;

namespace RadioM3U;

public class PluginConfiguration : BasePluginConfiguration
{
    public string M3UPath { get; set; } = "/config/radio.m3u";

    // Directory inside a Jellyfin library where the plugin will generate .strm items
    public string OutputLibraryPath { get; set; } = "/media/Stations";

    public bool UseTvgLogos { get; set; } = true;

    // Optional: default images per group (absolute paths or http urls). Example: { "Electro": "/media/stations/electro.jpg" }
    public Dictionary<string,string> GroupDefaultImages { get; set; } = new();
}
