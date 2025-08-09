#!/bin/bash

# Script per creare un tag Git e una release basati sulla versione nel file csproj

# Trova la versione nel file csproj
VERSION=$(grep -o '<Version>[^<]*</Version>' RadioM3U/RadioM3U.csproj | sed 's/<Version>\(.*\)<\/Version>/\1/')

# Aggiorna prima il manifest
./update-manifest.sh

# Verifica se ci sono modifiche da committare
if [[ $(git status -s) ]]; then
  echo "Ci sono modifiche da committare prima di creare il tag"
  git add manifest.json
  git commit -m "Aggiornato manifest.json alla versione v${VERSION}"
  git push origin main
fi

# Controlla se il tag esiste già
if git rev-parse "v${VERSION}" >/dev/null 2>&1; then
  echo "Il tag v${VERSION} esiste già"
else
  echo "Creazione del tag v${VERSION}..."
  git tag "v${VERSION}"
  git push origin "v${VERSION}"
  echo "Tag v${VERSION} creato e inviato a GitHub"
  echo "L'azione GitHub Actions dovrebbe ora compilare e creare il rilascio"
fi
