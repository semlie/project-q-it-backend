using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet("student/{userId}/overall")]
        public async Task<ActionResult<List<OverallStat>>> GetStudentOverallStats(int userId)
        {
            try
            {
                return Ok(await _statsService.GetStudentOverallStatsAsync(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving student overall stats");
            }
        }

        [HttpGet("student/{userId}/subjects")]
        public async Task<ActionResult<List<SubjectPerformanceItem>>> GetStudentSubjectPerformance(int userId, [FromQuery] string timeRange = "semester")
        {
            try
            {
                return Ok(await _statsService.GetStudentSubjectPerformanceAsync(userId, timeRange));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving subject performance");
            }
        }

        [HttpGet("student/{userId}/recent-tests")]
        public async Task<ActionResult<List<RecentTest>>> GetStudentRecentTests(int userId)
        {
            try
            {
                return Ok(await _statsService.GetStudentRecentTestsAsync(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving recent tests");
            }
        }

        [HttpGet("student/{userId}/study-habits")]
        public async Task<ActionResult<List<StudyHabits>>> GetStudentStudyHabits(int userId)
        {
            try
            {
                return Ok(await _statsService.GetStudentStudyHabitsAsync(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving study habits");
            }
        }

        [HttpGet("student/{userId}/achievements")]
        public async Task<ActionResult<List<Achievement>>> GetStudentAchievements(int userId)
        {
            try
            {
                return Ok(await _statsService.GetStudentAchievementsAsync(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving achievements");
            }
        }

        [HttpGet("student/{userId}/weekly-progress")]
        public async Task<ActionResult<List<WeeklyProgressItem>>> GetStudentWeeklyProgress(int userId)
        {
            try
            {
                return Ok(await _statsService.GetStudentWeeklyProgressAsync(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving weekly progress");
            }
        }

        [HttpGet("teacher/{userId}/overall")]
        public async Task<ActionResult<List<TeacherOverallStat>>> GetTeacherOverallStats(int userId)
        {
            try
            {
                return Ok(await _statsService.GetTeacherOverallStatsAsync(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving teacher overall stats");
            }
        }

        [HttpGet("teacher/{userId}/class-progress")]
        public async Task<ActionResult<List<ClassProgress>>> GetTeacherClassProgress(int userId)
        {
            try
            {
                return Ok(await _statsService.GetTeacherClassProgressAsync(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving class progress");
            }
        }

        [HttpGet("teacher/{userId}/subjects")]
        public async Task<ActionResult<List<TeacherSubjectItem>>> GetTeacherSubjects(int userId)
        {
            try
            {
                return Ok(await _statsService.GetTeacherSubjectsAsync(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving teacher subjects");
            }
        }

        [HttpGet("teacher/{userId}/recent-tests")]
        public async Task<ActionResult<List<RecentTest>>> GetTeacherRecentTests(int userId)
        {
            try
            {
                return Ok(await _statsService.GetTeacherRecentTestsAsync(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving teacher recent tests");
            }
        }
    }
}
