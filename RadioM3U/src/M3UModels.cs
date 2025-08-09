using System.Text.RegularExpressions;

namespace RadioM3U;

public record RadioStation(
    string Id,
    string Name,
    string Url,
    string? Group,
    string? LogoUrl
);

public static class M3UParser
{
    // Very small M3U EXTINF parser to read attributes like tvg-name, tvg-logo, group-title
    public static IEnumerable<RadioStation> Parse(string m3uContent)
    {
        var lines = m3uContent.Split('\n');
        string? extinf = null;
        foreach (var raw in lines.Select(l => l.Trim()))
        {
            if (string.IsNullOrWhiteSpace(raw)) continue;
            if (raw.StartsWith("#EXTINF"))
            {
                extinf = raw;
                continue;
            }
            if (raw.StartsWith("#")) continue; // other comments

            // url line
            if (extinf != null)
            {
                var name = GetAttr(extinf, "tvg-name") ?? GetTitle(extinf) ?? raw;
                var logo = GetAttr(extinf, "tvg-logo");
                var group = GetAttr(extinf, "group-title");
                var id = GetAttr(extinf, "tvg-id") ?? name.ToLowerInvariant().Replace(' ', '.');
                yield return new RadioStation(id, name, raw, group, logo);
                extinf = null;
            }
        }
    }

    static string? GetTitle(string extinf)
    {
        var idx = extinf.IndexOf(',');
        return idx >= 0 ? extinf[(idx+1)..].Trim() : null;
    }

    static string? GetAttr(string extinf, string attr)
    {
        // attr="value"
        var m = Regex.Match(extinf, attr + "=\"([^\"]+)\"");
        return m.Success ? m.Groups[1].Value : null;
    }
}
