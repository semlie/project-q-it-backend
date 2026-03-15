using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IStatsService
    {
        Task<List<OverallStat>> GetStudentOverallStatsAsync(int userId);
        Task<List<SubjectPerformanceItem>> GetStudentSubjectPerformanceAsync(int userId, string timeRange = "semester");
        Task<List<RecentTest>> GetStudentRecentTestsAsync(int userId);
        Task<List<StudyHabits>> GetStudentStudyHabitsAsync(int userId);
        Task<List<Achievement>> GetStudentAchievementsAsync(int userId);
        Task<List<WeeklyProgressItem>> GetStudentWeeklyProgressAsync(int userId);
        
        Task<List<TeacherOverallStat>> GetTeacherOverallStatsAsync(int userId);
        Task<List<ClassProgress>> GetTeacherClassProgressAsync(int userId);
        Task<List<TeacherSubjectItem>> GetTeacherSubjectsAsync(int userId);
        Task<List<RecentTest>> GetTeacherRecentTestsAsync(int userId);
    }

    public class OverallStat
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Change { get; set; }
        public string? Trend { get; set; }
    }

    public class SubjectPerformanceItem
    {
        public string Subject { get; set; } = string.Empty;
        public double Average { get; set; }
        public double LastGrade { get; set; }
        public string Trend { get; set; } = "stable";
        public int Tests { get; set; }
        public double ClassAverage { get; set; }
    }

    public class RecentTest
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public string Duration { get; set; } = string.Empty;
    }

    public class StudyHabits
    {
        public string Day { get; set; } = string.Empty;
        public double Hours { get; set; }
    }

    public class Achievement
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Type { get; set; } = "grade";
    }

    public class WeeklyProgressItem
    {
        public string Day { get; set; } = string.Empty;
        public int Tests { get; set; }
        public double Hours { get; set; }
        public double Average { get; set; }
    }

    public class TeacherOverallStat
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Change { get; set; }
        public string? Trend { get; set; }
    }

    public class ClassProgress
    {
        public string ClassName { get; set; } = string.Empty;
        public double Average { get; set; }
        public int Students { get; set; }
        public int Tests { get; set; }
        public string Trend { get; set; } = "stable";
    }

    public class TeacherSubjectItem
    {
        public string Subject { get; set; } = string.Empty;
        public int Classes { get; set; }
        public int Students { get; set; }
        public double AverageGrade { get; set; }
        public int TestsCreated { get; set; }
        public string Trend { get; set; } = "stable";
    }
}
