# Jellyfin RadioM3U Plugin

Plugin per Jellyfin che importa una playlist M3U e crea una libreria "Stations" con immagini e categorie. Mostra le stazioni come album e riproduce gli stream.

## Funzioni
- Import da file `.m3u` locale o URL remoto
- Parsing tag `#EXTINF` con `tvg-name`, `tvg-logo`, `group-title`
- Creazione di elementi Jellyfin di tipo Audio con metadata e cover
- Vista by group (Classic, Electro, Jazz, Hi‑Fi, Rock, All)

## Installazione rapida (build locale)
1. Prerequisiti: .NET 8 SDK
2. Build
   ```bash
   dotnet build -c Release
   ```
3. Copia l'output `bin/Release/net8.0/RadioM3U.dll` nella cartella plugin del server Jellyfin, ad es.:
   - Linux: `/var/lib/jellyfin/plugins/RadioM3U/`
   - macOS: `~/Library/Application Support/jellyfin/plugins/RadioM3U/`
4. Riavvia Jellyfin.

## Configurazione
- In Dashboard > Plugins > RadioM3U:
  - Path del file M3U (es. `/config/radio.m3u`)
  - Cartella immagini (opzionale). Se non presente usa i logo dal tag o fallback.

## Note
- Il plugin non scarica file audio; usa stream URL.
- Supporta AAC/MP3/FLAC se supportati dal server/clients.
