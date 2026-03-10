using Moq;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Xunit;

namespace server_q_it.Tests;

public class StatsControllerTests
{
    private readonly Mock<IStatsService> _mockStatsService;
    private readonly webApiProject.Controllers.StatsController _controller;

    public StatsControllerTests()
    {
        _mockStatsService = new Mock<IStatsService>();
        _controller = new webApiProject.Controllers.StatsController(_mockStatsService.Object);
    }

    [Fact]
    public void GetStudentOverallStats_ReturnsOk()
    {
        var stats = new List<OverallStat>
        {
            new() { Label = "ממוצע", Value = "85%" }
        };
        _mockStatsService.Setup(s => s.GetStudentOverallStats(1)).Returns(stats);

        var result = _controller.GetStudentOverallStats(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void GetStudentSubjectPerformance_ReturnsOk()
    {
        var subjects = new List<SubjectPerformanceItem>
        {
            new() { Subject = "Math", Average = 85 }
        };
        _mockStatsService.Setup(s => s.GetStudentSubjectPerformance(1, "semester")).Returns(subjects);

        var result = _controller.GetStudentSubjectPerformance(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void GetStudentRecentTests_ReturnsOk()
    {
        var tests = new List<RecentTest>
        {
            new() { Id = 1, Subject = "Math", Title = "Test 1" }
        };
        _mockStatsService.Setup(s => s.GetStudentRecentTests(1)).Returns(tests);

        var result = _controller.GetStudentRecentTests(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void GetStudentStudyHabits_ReturnsOk()
    {
        var habits = new List<StudyHabits>
        {
            new() { Day = "Sunday", Hours = 2 }
        };
        _mockStatsService.Setup(s => s.GetStudentStudyHabits(1)).Returns(habits);

        var result = _controller.GetStudentStudyHabits(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void GetStudentAchievements_ReturnsOk()
    {
        var achievements = new List<Achievement>
        {
            new() { Id = 1, Title = "First Test" }
        };
        _mockStatsService.Setup(s => s.GetStudentAchievements(1)).Returns(achievements);

        var result = _controller.GetStudentAchievements(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void GetStudentWeeklyProgress_ReturnsOk()
    {
        var progress = new List<WeeklyProgressItem>
        {
            new() { Day = "Monday", Tests = 2 }
        };
        _mockStatsService.Setup(s => s.GetStudentWeeklyProgress(1)).Returns(progress);

        var result = _controller.GetStudentWeeklyProgress(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void GetTeacherOverallStats_ReturnsOk()
    {
        var stats = new List<TeacherOverallStat>
        {
            new() { Label = "Classes", Value = "5" }
        };
        _mockStatsService.Setup(s => s.GetTeacherOverallStats(1)).Returns(stats);

        var result = _controller.GetTeacherOverallStats(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void GetTeacherClassProgress_ReturnsOk()
    {
        var classes = new List<ClassProgress>
        {
            new() { ClassName = "Class A", Average = 80 }
        };
        _mockStatsService.Setup(s => s.GetTeacherClassProgress(1)).Returns(classes);

        var result = _controller.GetTeacherClassProgress(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void GetTeacherSubjects_ReturnsOk()
    {
        var subjects = new List<TeacherSubjectItem>
        {
            new() { Subject = "Math", Classes = 3 }
        };
        _mockStatsService.Setup(s => s.GetTeacherSubjects(1)).Returns(subjects);

        var result = _controller.GetTeacherSubjects(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void GetTeacherRecentTests_ReturnsOk()
    {
        var tests = new List<RecentTest>
        {
            new() { Id = 1, Subject = "Math" }
        };
        _mockStatsService.Setup(s => s.GetTeacherRecentTests(1)).Returns(tests);

        var result = _controller.GetTeacherRecentTests(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }
}
