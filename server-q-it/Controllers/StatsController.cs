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
        public ActionResult<List<OverallStat>> GetStudentOverallStats(int userId)
        {
            try
            {
                return Ok(_statsService.GetStudentOverallStats(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving student overall stats");
            }
        }

        [HttpGet("student/{userId}/subjects")]
        public ActionResult<List<SubjectPerformanceItem>> GetStudentSubjectPerformance(int userId, [FromQuery] string timeRange = "semester")
        {
            try
            {
                return Ok(_statsService.GetStudentSubjectPerformance(userId, timeRange));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving subject performance");
            }
        }

        [HttpGet("student/{userId}/recent-tests")]
        public ActionResult<List<RecentTest>> GetStudentRecentTests(int userId)
        {
            try
            {
                return Ok(_statsService.GetStudentRecentTests(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving recent tests");
            }
        }

        [HttpGet("student/{userId}/study-habits")]
        public ActionResult<List<StudyHabits>> GetStudentStudyHabits(int userId)
        {
            try
            {
                return Ok(_statsService.GetStudentStudyHabits(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving study habits");
            }
        }

        [HttpGet("student/{userId}/achievements")]
        public ActionResult<List<Achievement>> GetStudentAchievements(int userId)
        {
            try
            {
                return Ok(_statsService.GetStudentAchievements(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving achievements");
            }
        }

        [HttpGet("student/{userId}/weekly-progress")]
        public ActionResult<List<WeeklyProgressItem>> GetStudentWeeklyProgress(int userId)
        {
            try
            {
                return Ok(_statsService.GetStudentWeeklyProgress(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving weekly progress");
            }
        }

        [HttpGet("teacher/{userId}/overall")]
        public ActionResult<List<TeacherOverallStat>> GetTeacherOverallStats(int userId)
        {
            try
            {
                return Ok(_statsService.GetTeacherOverallStats(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving teacher overall stats");
            }
        }

        [HttpGet("teacher/{userId}/class-progress")]
        public ActionResult<List<ClassProgress>> GetTeacherClassProgress(int userId)
        {
            try
            {
                return Ok(_statsService.GetTeacherClassProgress(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving class progress");
            }
        }

        [HttpGet("teacher/{userId}/subjects")]
        public ActionResult<List<TeacherSubjectItem>> GetTeacherSubjects(int userId)
        {
            try
            {
                return Ok(_statsService.GetTeacherSubjects(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving teacher subjects");
            }
        }

        [HttpGet("teacher/{userId}/recent-tests")]
        public ActionResult<List<RecentTest>> GetTeacherRecentTests(int userId)
        {
            try
            {
                return Ok(_statsService.GetTeacherRecentTests(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving teacher recent tests");
            }
        }
    }
}
