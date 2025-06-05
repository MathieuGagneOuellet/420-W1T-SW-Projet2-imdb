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
            [FromQuery] string? genre
        )
        {
            if (string.IsNullOrWhiteSpace(type))
                return BadRequest("Le type est requis");

            var results = await _dao.GetByCriteriaAsync(type, annee, genre);
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

    }
}