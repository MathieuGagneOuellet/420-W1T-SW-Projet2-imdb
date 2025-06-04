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
    }
}