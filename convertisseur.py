#Script python généré par ChatGPT pour convertir mon fichier Excel "movies filtrés.xlsx" en fichier .json que je vais pouvoir directement utiliser dans MongoDB
#J'ai juste à ouvrir mon terminal dans VS Code et faire python convertisseur.py, boom j'ai un beau fichier media_items_1200.json
#L'énoncé demande MAX 2000 enregistrement, j'ai choisi d'en inclure 1200.


import pandas as pd
import json

# Charger le fichier Excel
df = pd.read_excel("movies filtrés.xlsx")

# Renommer les colonnes pour correspondre au JSON final
df = df.rename(columns={
    "tconst": "_id",
    "primaryTitle": "titre",
    "titleType": "type",
    "startYear": "annee",
    "runtimeMinutes": "duree"
})

# Convertir les colonnes numériques
df["annee"] = pd.to_numeric(df["annee"], errors="coerce").fillna(0).astype(int)
df["duree"] = pd.to_numeric(df["duree"], errors="coerce").fillna(0).astype(int)

# Transformer les genres en liste
df["genres"] = df["genres"].fillna("").apply(lambda g: [genre.strip() for genre in g.split(",") if genre.strip() != ""])

# Garder seulement les colonnes utiles
df = df[["_id", "titre", "type", "annee", "duree", "genres"]]

# Exporter en JSON
with open("media_items_1200.json", "w", encoding="utf-8") as f:
    json.dump(df.to_dict(orient="records"), f, ensure_ascii=False, indent=2)

print("✅ Fichier JSON généré.")