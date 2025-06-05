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
        private readonly MediaStatusDAO _mediaStatusDAO;
        private readonly MediaItemDAO _mediaItemDAO;

        //constructeur
        public UserController(UserDAO dao, MediaStatusDAO mediaStatusDAO, MediaItemDAO mediaItemDAO)
        {
            _dao = dao;
            _mediaStatusDAO = mediaStatusDAO;
            _mediaItemDAO = mediaItemDAO;
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

        //route qui appelle GetByIdsAsync pour pouvoir retourner tous les mediaItems liés à un user
        [HttpGet("{userId}/medias")]
        public async Task<ActionResult<List<MediaItemWithStatus>>> GetUserMedias(string userId)
        {
            var statuses = await _mediaStatusDAO.GetByUserIdAsync(userId);
            if (statuses == null || statuses.Count == 0)
                return NotFound("Aucun lien trouvé avec ce user.");

            var mediaIds = statuses.Select(s => s.MediaId).Distinct().ToList();
            var mediaItems = await _mediaItemDAO.GetByIdsAsync(mediaIds);

            var combined = statuses
                .Select(status =>
                {
                    var media = mediaItems.FirstOrDefault(m => m.Id == status.MediaId);
                    return media != null
                        ? new MediaItemWithStatus { Media = media, Status = status.Status }
                        : null;
                })
                .Where(x => x != null)
                .ToList()!;

            return Ok(combined);
        }

        //route qui appelle GetByIdsAsync pour pouvoir retourner tous les mediaItems dans la Watchlist du user (par exemple)
        [HttpGet("{userId}/medias/{status}")]
        public async Task<ActionResult<List<MediaItemWithStatus>>> GetUserMediasByStatus(string userId, string status)
        {
            var statuses = await _mediaStatusDAO.GetByUserAndStatusAsync(userId, status);
            if (statuses == null || statuses.Count == 0)
                return NotFound("Aucun lien trouvé avec ce statut.");

            var mediaIds = statuses.Select(s => s.MediaId).Distinct().ToList();
            var mediaItems = await _mediaItemDAO.GetByIdsAsync(mediaIds);

            var combined = statuses
                .Select(s =>
                {
                    var media = mediaItems.FirstOrDefault(m => m.Id == s.MediaId);
                    return media != null
                        ? new MediaItemWithStatus { Media = media, Status = s.Status }
                        : null;
                })
                .Where(x => x != null)
                .ToList()!;

            return Ok(combined);
        }


    }    
}