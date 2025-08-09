#!/bin/bash

# Script per aggiornare automaticamente il manifest.json con la versione dal file csproj

# Trova la versione nel file csproj
VERSION=$(grep -o '<Version>[^<]*</Version>' RadioM3U/RadioM3U.csproj | sed 's/<Version>\(.*\)<\/Version>/\1/')

# Aggiorna la data e ora corrente
TIMESTAMP=$(date -u +"%Y-%m-%dT%H:%M:%SZ")

# Variabili per il percorso del file ZIP
PROJECT_NAME="RadioM3U"
ZIP_FILENAME="${PROJECT_NAME}_v${VERSION}.zip"
DOWNLOAD_PATH="https://github.com/ahelja/jellyfin-radio-m3u-plugin/releases/download/v${VERSION}/${ZIP_FILENAME}"

# Aggiorna la versione nel manifest.json
sed -i '' "s/\"version\": \"[0-9]*\.[0-9]*\.[0-9]*\"/\"version\": \"$VERSION\"/" manifest.json
sed -i '' "s/\"timestamp\": \"[^\"]*\"/\"timestamp\": \"$TIMESTAMP\"/" manifest.json

# Aggiorna la versione nell'array assets
sed -i '' "s/\"version\": \"[0-9]*\.[0-9]*\.[0-9]*\",/\"version\": \"$VERSION\",/" manifest.json

# Aggiorna l'URL del file ZIP
sed -i '' "s|\"assetUrl\": \"https://github.com/ahelja/jellyfin-radio-m3u-plugin/releases/download/v[^/]*/[^\"]*\"|\"assetUrl\": \"$DOWNLOAD_PATH\"|" manifest.json

echo "Manifest aggiornato con la versione $VERSION e timestamp $TIMESTAMP"
echo "URL del file ZIP aggiornato a: $DOWNLOAD_PATH"
