# Jellyfin RadioM3U Plugin

A Jellyfin plugin that imports an M3U playlist and creates a "Stations" library with images and categories. It displays stations as albums and plays their streams.

## Features
- Import from a local `.m3u` file or a remote URL
- Parse `#EXTINF` tags with `tvg-name`, `tvg-logo`, `group-title`
- Create Jellyfin Audio items with metadata and cover art
- Group view (Classic, Electro, Jazz, Hi‑Fi, Rock, All)

## Installation

### Method 1: Via GitHub Repository (Recommended)
1. Open Jellyfin Dashboard
2. Go to "Plugins" → "Repositories"
3. Add this URL:
   ```
   https://raw.githubusercontent.com/ahelja/jellyfin-radio-m3u-plugin/main/manifest.json
   ```
4. Go to "Catalog" and install "RadioM3U"
5. Restart Jellyfin

### Method 2: Download Release
1. Download the latest release from [GitHub Releases](https://github.com/ahelja/jellyfin-radio-m3u-plugin/releases)
2. Extract the ZIP into Jellyfin’s plugins folder:
   - Linux: `/var/lib/jellyfin/plugins/RadioM3U/`
   - macOS: `~/Library/Application Support/jellyfin/plugins/RadioM3U/`
   - Windows: `%ProgramData%\Jellyfin\Server\plugins\RadioM3U\`
3. Restart Jellyfin

### Method 3: Local Build
1. Prerequisites: .NET 8 SDK
2. Build
   ```bash
   git clone https://github.com/ahelja/jellyfin-radio-m3u-plugin.git
   cd jellyfin-radio-m3u-plugin
   dotnet build RadioM3U/RadioM3U.csproj -c Release
   ```
3. Copy `RadioM3U/bin/Release/net8.0/RadioM3U.dll` to the Jellyfin server plugins folder
4. Restart Jellyfin

## Automated Builds with GitHub
This project includes GitHub Actions for automated builds.

### For developers — Create a release
```bash
# Commit changes
git add .
git commit -m "Release v0.1.0"

# Create and push the tag to trigger the release
git tag v0.1.0
git push origin main --tags
```

The GitHub Actions workflow:
- Builds the plugin
- Creates the distribution ZIP
- Publishes the release on GitHub
- Updates the manifest with the correct checksum

## Configuration
- Dashboard > Plugins > RadioM3U:
  - M3U file path (e.g., `/config/radio.m3u`)
  - Images folder (optional). If not present, uses logos from tags or a fallback.

## Notes
- The plugin does not download audio files; it uses stream URLs.
- Supports AAC/MP3/FLAC if supported by the server/clients.
