using imdbexperience.DAL.DAO;
using Microsoft.AspNetCore.Mvc;

namespace imdbexperience.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly StatsDAO _dao;
        public StatsController(StatsDAO dao)
        {
            _dao = dao;
        }
        [HttpGet("totals")]
        public async Task<ActionResult<object>> GetGlobalCounts() //totals retourne Users, MediaItems et MediaStatuses
        {
            var userCount = await _dao.GetUserCountAsync();
            var mediaCount = await _dao.GetMediaItemCountAsync();
            var statusCount = await _dao.GetMediaStatusCountAsync();

            return Ok(new
            {
                Users = userCount,
                MediaItems = mediaCount,
                MediaStatuses = statusCount
            });
        }
        [HttpGet("statuses")]
        public async Task<ActionResult<Dictionary<string, int>>> GetStatusBreakdown() //retourne {"seen":3, "favorite":1, ...}
        {
            var statusCounts = await _dao.CountByStatusAsync();
            return Ok(statusCounts);
        }

        [HttpGet("user/{userId}/topgenres")]
        public async Task<ActionResult<Dictionary<string, int>>> GetTopGenres(string userId, [FromServices] MediaItemDAO mediaItemDao)
        {
            var result = await _dao.GetFavGenresForUserAsync(userId, mediaItemDao);
            return Ok(result);
        }
        
        [HttpGet("{userId}/average-duration")]
        public async Task<ActionResult<double>> GetAverageDuration(string userId)
        {
            var avg = await _dao.GetAverageDurationSeenAsync(userId);
            return Ok(avg);
        }

        [HttpGet("{userId}/activity-by-year")]
        public async Task<ActionResult<Dictionary<int, int>>> GetActivityByYear(string userId)
        {
            var stats = await _dao.GetSeenCountByYearAsync(userId);
            return Ok(stats);
        }

    }
}
