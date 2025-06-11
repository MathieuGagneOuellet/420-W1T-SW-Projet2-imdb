using imdbexperience.DAL.DAO;
using imdbexperience.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace imdbexperience.Controllers
{
    //controlleur simple qui injecte le dao et une route GetAll et GetById de base
    [ApiController]
    [Route("api/[controller]")]
    public class MediaItemController : ControllerBase
    {
        private readonly MediaItemDAO _dao;

        //constructeur
        public MediaItemController(MediaItemDAO dao)
        {
            _dao = dao;
        }

        [HttpGet]
        public async Task<ActionResult<List<MediaItem>>> GetAll()
        {
            var items = await _dao.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MediaItem>> GetById(string id)
        {
            var item = await _dao.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("recherche")]
        public async Task<ActionResult<List<MediaItem>>> GetByCriteria
        (
            [FromQuery] string type,
            [FromQuery] int? annee,
            [FromQuery] string? genre,
            [FromQuery] string? keyword
        )
        {
            if (string.IsNullOrWhiteSpace(type))
                return BadRequest("Le type est requis");

            var results = await _dao.GetByCriteriaAsync(type, annee, genre, keyword);
            return Ok(results);
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MediaItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Id))
                return BadRequest("Un item valide avec un _id est requis");

            await _dao.InsertAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] MediaItem updatedItem)
        {
            if (updatedItem == null || updatedItem.Id != id)
                return BadRequest("Erreur dans la requête envoyée");

            var success = await _dao.UpdateAsync(updatedItem);
            if (!success) return NotFound();

            return NoContent(); //response serveur 204 : succès mais pas de retour
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var success = await _dao.DeleteAsync(id);
            if (!success) return NotFound();

            return NoContent(); //204 si suppression OK
        }

        [HttpGet("{id}/genres")]
        public async Task<ActionResult<List<Genre>>> GetGenresForMedia(string id, [FromServices] GenreDAO genreDao)
        {
            var media = await _dao.GetByIdAsync(id);
            if (media == null) return NotFound("Média introuvable");

            var genres = await genreDao.GetByNamesAsync(media.Genres);
            return Ok(genres);
        }

        [HttpGet("partial")]
        public async Task<ActionResult<List<object>>> GetPartial()
        {
            var items = await _dao.GetPartialAsync();
            return Ok(items);
        }

        [HttpPost("complet")]
        public async Task<ActionResult> CreateWithGenres([FromBody] MediaItem item, [FromServices] GenreDAO genreDao)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Id))
                return BadRequest("Objet MediaItem invalide");

            var success = await _dao.CreateMediaWithGenresAsync(item, genreDao);
            return success
                ? CreatedAtAction(nameof(GetById), new { id = item.Id }, item)
                : StatusCode(500, "Erreur lors de la création");
        }

        [HttpPost("transaction")]
        public async Task<ActionResult> CreateWithGenresTransactional([FromBody] MediaItem item, [FromServices] GenreDAO genreDao)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Id) || item.Genres == null || item.Genres.Count == 0)
                return BadRequest("Objet MediaItem invalide");

            var success = await _dao.CreateMediaGenreTransaction(item, item.Genres, genreDao);

            if (!success)
                return StatusCode(500, "Erreur pendant la transaction");

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

    }
}