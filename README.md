# Jellyfin RadioM3U Plugin

Plugin per Jellyfin che importa una playlist M3U e crea una libreria "Stations" con immagini e categorie. Mostra le stazioni come album e riproduce gli stream.

## Funzioni
- Import da file `.m3u` locale o URL remoto
- Parsing tag `#EXTINF` con `tvg-name`, `tvg-logo`, `group-title`
- Creazione di elementi Jellyfin di tipo Audio con metadata e cover
- Vista by group (Classic, Electro, Jazz, Hi‑Fi, Rock, All)

## Installazione

### Metodo 1: Tramite Repository GitHub (Consigliato)

1. Apri Jellyfin Dashboard
2. Vai su "Plugins" → "Repositories"
3. Aggiungi questo URL:
   ```
   https://raw.githubusercontent.com/ahelja/jellyfin-radio-m3u-plugin/main/manifest.json
   ```
4. Vai su "Catalog" e installa "RadioM3U"
5. Riavvia Jellyfin

### Metodo 2: Download Release
1. Scarica l'ultimo release da [GitHub Releases](https://github.com/ahelja/jellyfin-radio-m3u-plugin/releases)
2. Estrai il file ZIP nella cartella plugins di Jellyfin:
   - Linux: `/var/lib/jellyfin/plugins/RadioM3U/`
   - macOS: `~/Library/Application Support/jellyfin/plugins/RadioM3U/`
   - Windows: `%ProgramData%\Jellyfin\Server\plugins\RadioM3U\`
3. Riavvia Jellyfin

### Metodo 3: Build Locale
1. Prerequisiti: .NET 8 SDK
2. Build
   ```bash
   git clone https://github.com/ahelja/jellyfin-radio-m3u-plugin.git
   cd jellyfin-radio-m3u-plugin
   dotnet build RadioM3U/RadioM3U.csproj -c Release
   ```
3. Copia l'output `RadioM3U/bin/Release/net8.0/RadioM3U.dll` nella cartella plugin del server Jellyfin
4. Riavvia Jellyfin

## Compilazione Automatica con GitHub

Il progetto include GitHub Actions per la compilazione automatica:

### Per sviluppatori - Creare una release:

```bash
# Committa le modifiche
git add .
git commit -m "Release v0.1.0"

# Crea e pusha il tag per trigger la release
git tag v0.1.0
git push origin main --tags
```

Il workflow GitHub Actions:
- Compila automaticamente il plugin
- Crea il file ZIP di distribuzione
- Pubblica la release su GitHub
- Aggiorna il manifest con il checksum corretto

## Configurazione
- In Dashboard > Plugins > RadioM3U:
  - Path del file M3U (es. `/config/radio.m3u`)
  - Cartella immagini (opzionale). Se non presente usa i logo dal tag o fallback.

## Note
- Il plugin non scarica file audio; usa stream URL.
- Supporta AAC/MP3/FLAC se supportati dal server/clients.
