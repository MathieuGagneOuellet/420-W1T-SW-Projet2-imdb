using imdbexperience.DAL.DAO;
using imdbexperience.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace imdbexperience.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly GenreDAO _dao;

        //constructeur
        public GenreController(GenreDAO dao)
        {
            _dao = dao;
        }

        [HttpGet]
        public async Task<ActionResult<List<Genre>>> GetAll()
        {
            var genres = await _dao.GetAllAsync();
            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetById(string id)
        {
            var genre = await _dao.GetByIdAsync(id);
            if (genre == null) return NotFound();
            return Ok(genre);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Genre genre)
        {
            await _dao.CreateAsync(genre);
            return CreatedAtAction(nameof(GetById), new { id = genre.Id }, genre);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] Genre genre)
        {
            if (genre == null || genre.Id != id)
                return BadRequest();

            var success = await _dao.UpdateAsync(genre);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var success = await _dao.DeleteAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
