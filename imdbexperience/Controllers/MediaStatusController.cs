using imdbexperience.DAL.DAO;
using imdbexperience.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace imdbexperience.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaStatusController : ControllerBase
    {
        private readonly MediaStatusDAO _dao;

        //constructeur
        public MediaStatusController(MediaStatusDAO dao)
        {
            _dao = dao;
        }

        [HttpGet]
        public async Task<ActionResult<List<MediaStatus>>> GetAll()
        {
            var statuses = await _dao.GetAllAsync();
            return Ok(statuses);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<MediaStatus>>> GetByUser(string userId)
        {
            var statuses = await _dao.GetByUserIdAsync(userId);
            return Ok(statuses);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MediaStatus status)
        {
            if (string.IsNullOrWhiteSpace(status.UserId) || string.IsNullOrWhiteSpace(status.MediaId))
                return BadRequest("Erreur avec la requête");

            await _dao.CreateAsync(status);
            return Created(string.Empty, status);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var success = await _dao.DeleteAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] MediaStatus updatedStatus)
        {
            if (updatedStatus == null || updatedStatus.Id != id)
                return BadRequest("Erreur de requête au niveau de l'identifiant");

            var success = await _dao.UpdateAsync(updatedStatus);
            if (!success)
                return NotFound();

            return NoContent();
        }        
    }    
}