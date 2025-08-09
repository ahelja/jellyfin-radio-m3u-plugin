#!/bin/bash

# Script per aggiornare automaticamente il manifest.json con la versione dal file csproj

# Trova la versione nel file csproj
VERSION=$(grep -o '<Version>[^<]*</Version>' RadioM3U/RadioM3U.csproj | sed 's/<Version>\(.*\)<\/Version>/\1/')

# Aggiorna la data e ora corrente
TIMESTAMP=$(date -u +"%Y-%m-%dT%H:%M:%SZ")

# Aggiorna la versione nel manifest.json
sed -i '' "s/\"version\": \"[0-9]*\.[0-9]*\.[0-9]*\"/\"version\": \"$VERSION\"/" manifest.json
sed -i '' "s/\"timestamp\": \"[^\"]*\"/\"timestamp\": \"$TIMESTAMP\"/" manifest.json
sed -i '' "s/v[0-9]*\.[0-9]*\.[0-9]*/v$VERSION/g" manifest.json

echo "Manifest aggiornato con la versione $VERSION e timestamp $TIMESTAMP"
