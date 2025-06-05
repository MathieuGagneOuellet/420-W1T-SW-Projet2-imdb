using imdbexperience.DAL.DAO;
using imdbexperience.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace imdbexperience.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserDAO _dao;

        public UserController(UserDAO dao)
        {
            _dao = dao;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _dao.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            var user = await _dao.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] User user)
        {
            await _dao.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] User user)
        {
            if (user == null || user.Id != id)
                return BadRequest("Erreur de requête");

            var success = await _dao.UpdateAsync(user);
            if (!success) return NotFound();

            return NoContent();
        }

    }    
}