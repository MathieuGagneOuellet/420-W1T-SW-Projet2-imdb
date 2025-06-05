//Combo de MediaItem et Status pour filtres
//Par exemple, cet entité est necessaire pour que la route /api/user/{userId}/medias retourne 
//"media"{son bloc},
//"status" : "seen"
//"status" : "favorite"

namespace imdbexperience.DAL.Entities
{
    public class MediaItemWithStatus
    {
        public MediaItem Media { get; set; } = new();
        public string Status { get; set; } = string.Empty;
    }
}