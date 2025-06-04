//script pour prendre media_items_1200.json et l'insérer dans db.mediaItems
use("imdb");
const data = require("./media_items_1200.json");
db.mediaItems.insertMany(data);
