using Repository.Entities;
using Repository.interfaces;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class StatsService : IStatsService
    {
        private readonly IContext _context;

        public StatsService(IContext context)
        {
            _context = context;
        }

        public async Task<List<OverallStat>> GetStudentOverallStatsAsync(int userId)
        {
            var tests = _context.Set<TestResult>().Where(t => t.StudentId == userId).ToList();
            var student = _context.Set<Users>().FirstOrDefault(u => u.UserId == userId);
            
            var totalTests = tests.Count;
            var avgScore = totalTests > 0 ? tests.Sum(t => (double)t.Score / t.MaxScore * 100) / totalTests : 0;
            var totalHours = tests.Sum(t => t.Duration) / 60.0;
            var subjectsCount = tests.Select(t => t.Subject).Distinct().Count();

            return await Task.FromResult(new List<OverallStat>
            {
                new OverallStat { Label = "ממוצע כללי", Value = $"{avgScore:F1}%", Trend = avgScore >= 70 ? "up" : "down" },
                new OverallStat { Label = "מבחנים שהושלמו", Value = totalTests.ToString(), Trend = "up" },
                new OverallStat { Label = "שעות למידה", Value = $"{totalHours:F1}h", Trend = "up" },
                new OverallStat { Label = "מקצועות", Value = subjectsCount.ToString(), Trend = "stable" }
            });
        }

        public async Task<List<SubjectPerformanceItem>> GetStudentSubjectPerformanceAsync(int userId, string timeRange = "semester")
        {
            var tests = _context.Set<TestResult>().Where(t => t.StudentId == userId).ToList();
            
            return await Task.FromResult(tests
                .GroupBy(t => t.Subject)
                .Select(g => new SubjectPerformanceItem
                {
                    Subject = g.Key,
                    Average = g.Average(t => (double)t.Score / t.MaxScore * 100),
                    LastGrade = g.OrderByDescending(t => t.Date).First().Score * 100.0 / g.OrderByDescending(t => t.Date).First().MaxScore,
                    Trend = "stable",
                    Tests = g.Count(),
                    ClassAverage = g.Average(t => (double)t.Score / t.MaxScore * 100)
                })
                .ToList());
        }

        public async Task<List<RecentTest>> GetStudentRecentTestsAsync(int userId)
        {
            return await Task.FromResult(_context.Set<TestResult>()
                .Where(t => t.StudentId == userId)
                .OrderByDescending(t => t.Date)
                .Take(10)
                .Select(t => new RecentTest
                {
                    Id = t.TestResultId,
                    Subject = t.Subject,
                    Title = t.Title,
                    Date = t.Date,
                    Score = t.Score,
                    MaxScore = t.MaxScore,
                    Duration = $"{t.Duration} דקות"
                })
                .ToList());
        }

        public async Task<List<StudyHabits>> GetStudentStudyHabitsAsync(int userId)
        {
            var tests = _context.Set<TestResult>().Where(t => t.StudentId == userId).ToList();
            
            return await Task.FromResult(tests
                .GroupBy(t => t.Date.DayOfWeek)
                .Select(g => new StudyHabits
                {
                    Day = g.Key.ToString("d"),
                    Hours = g.Sum(t => t.Duration) / 60.0
                })
                .OrderBy(s => Array.IndexOf(new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" }, s.Day))
                .ToList());
        }

        public async Task<List<Achievement>> GetStudentAchievementsAsync(int userId)
        {
            var tests = _context.Set<TestResult>().Where(t => t.StudentId == userId).ToList();
            var achievements = new List<Achievement>();
            
            if (tests.Count >= 10)
            {
                achievements.Add(new Achievement
                {
                    Id = 1,
                    Title = "מתחיל מצוין",
                    Description = "השלמת 10 מבחנים",
                    Icon = "trophy",
                    Date = tests.OrderByDescending(t => t.Date).First().Date,
                    Type = "streak"
                });
            }

            var avgScore = tests.Count > 0 ? tests.Average(t => (double)t.Score / t.MaxScore * 100) : 0;
            if (avgScore >= 90)
            {
                achievements.Add(new Achievement
                {
                    Id = 2,
                    Title = "מצטיין",
                    Description = "ממוצע מעל 90%",
                    Icon = "star",
                    Date = DateTime.Now,
                    Type = "grade"
                });
            }

            return await Task.FromResult(achievements);
        }

        public async Task<List<WeeklyProgressItem>> GetStudentWeeklyProgressAsync(int userId)
        {
            var oneWeekAgo = DateTime.Now.AddDays(-7);
            var tests = _context.Set<TestResult>()
                .Where(t => t.StudentId == userId && t.Date >= oneWeekAgo)
                .ToList();

            var days = new[] { "ראשון", "שני", "שלישי", "רביעי", "חמישי", "שישי", "שבת" };
            
            return await Task.FromResult(days.Select(day => new WeeklyProgressItem
            {
                Day = day,
                Tests = tests.Count(t => t.Date.DayOfWeek.ToString("d") == Array.IndexOf(days, day).ToString()),
                Hours = tests.Where(t => t.Date.DayOfWeek.ToString("d") == Array.IndexOf(days, day).ToString()).Sum(t => t.Duration) / 60.0,
                Average = tests.Where(t => t.Date.DayOfWeek.ToString("d") == Array.IndexOf(days, day).ToString()).Select(t => (double)t.Score / t.MaxScore * 100).DefaultIfEmpty(0).Average()
            }).ToList());
        }

        public async Task<List<TeacherOverallStat>> GetTeacherOverallStatsAsync(int userId)
        {
            var teacherClasses = _context.Set<TeacherClass>().Where(tc => tc.TeacherId == userId).ToList();
            var classIds = teacherClasses.Select(tc => tc.ClassId).ToList();
            var students = _context.Set<Users>().Where(u => classIds.Contains(u.ClassId ?? 0)).ToList();
            var tests = _context.Set<TestResult>().Where(t => students.Select(s => s.UserId).Contains(t.StudentId)).ToList();

            return await Task.FromResult(new List<TeacherOverallStat>
            {
                new TeacherOverallStat { Label = "כיתות", Value = teacherClasses.Count.ToString() },
                new TeacherOverallStat { Label = "תלמידים", Value = students.Count.ToString() },
                new TeacherOverallStat { Label = "מבחנים", Value = tests.Count.ToString() },
                new TeacherOverallStat { Label = "ממוצע כללי", Value = tests.Count > 0 ? $"{tests.Average(t => (double)t.Score / t.MaxScore * 100):F1}%" : "0%" }
            });
        }

        public async Task<List<ClassProgress>> GetTeacherClassProgressAsync(int userId)
        {
            var teacherClasses = _context.Set<TeacherClass>().Where(tc => tc.TeacherId == userId).ToList();
            
            return await Task.FromResult(teacherClasses.Select(tc =>
            {
                var classStudents = _context.Set<Users>().Where(u => u.ClassId == tc.ClassId).ToList();
                var studentIds = classStudents.Select(s => s.UserId).ToList();
                var tests = _context.Set<TestResult>().Where(t => studentIds.Contains(t.StudentId)).ToList();

                return new ClassProgress
                {
                    ClassName = tc.Class?.ClassName ?? "כיתה " + tc.ClassId,
                    Average = tests.Count > 0 ? tests.Average(t => (double)t.Score / t.MaxScore * 100) : 0,
                    Students = classStudents.Count,
                    Tests = tests.Count,
                    Trend = "stable"
                };
            }).ToList());
        }

        public async Task<List<TeacherSubjectItem>> GetTeacherSubjectsAsync(int userId)
        {
            var teacherClasses = _context.Set<TeacherClass>().Where(tc => tc.TeacherId == userId).ToList();
            var classIds = teacherClasses.Select(tc => tc.ClassId).ToList();
            var studentIds = _context.Set<Users>().Where(u => u.ClassId != null && classIds.Contains(u.ClassId ?? 0)).Select(u => u.UserId).ToList();
            
            var testResults = _context.Set<TestResult>().Where(t => studentIds.Contains(t.StudentId)).ToList();
            
            var subjects = testResults
                .Select(t => t.Subject)
                .Distinct()
                .ToList();

            return await Task.FromResult(subjects.Select(s => {
                var subjectTests = testResults.Where(t => t.Subject == s).ToList();
                return new TeacherSubjectItem
                {
                    Subject = s,
                    Classes = teacherClasses.Count,
                    Students = studentIds.Count(),
                    AverageGrade = subjectTests.Count > 0 ? subjectTests.Average(t => (double)t.Score / t.MaxScore * 100) : 0,
                    TestsCreated = subjectTests.Count,
                    Trend = "stable"
                };
            }).ToList());
        }

        public async Task<List<RecentTest>> GetTeacherRecentTestsAsync(int userId)
        {
            var teacherClasses = _context.Set<TeacherClass>().Where(tc => tc.TeacherId == userId).ToList();
            var classIds = teacherClasses.Select(tc => tc.ClassId).ToList();
            var studentIds = _context.Set<Users>().Where(u => classIds.Contains(u.ClassId ?? 0)).Select(u => u.UserId).ToList();

            return await Task.FromResult(_context.Set<TestResult>()
                .Where(t => studentIds.Contains(t.StudentId))
                .OrderByDescending(t => t.Date)
                .Take(10)
                .Select(t => new RecentTest
                {
                    Id = t.TestResultId,
                    Subject = t.Subject,
                    Title = t.Title,
                    Date = t.Date,
                    Score = t.Score,
                    MaxScore = t.MaxScore,
                    Duration = $"{t.Duration} דקות"
                })
                .ToList());
        }
    }
}
