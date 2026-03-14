using Microsoft.EntityFrameworkCore;
using CodeFirst.Models;
using Repository.Entities;
using Microsoft.Extensions.Configuration;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Try to read connection string from this project's configuration first,
// otherwise fall back to the repository root appsettings.json or environment variable.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    // Try a few common locations and environment variable
    var cfgBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true)
        .AddJsonFile(Path.Combine("..", "appsettings.json"), optional: true)
        .AddJsonFile(Path.Combine("..", "server-q-it", "appsettings.json"), optional: true)
        .AddJsonFile(Path.Combine("..", "..", "appsettings.json"), optional: true)
        .AddEnvironmentVariables();

    var cfg = cfgBuilder.Build();
    connectionString = cfg.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        // Direct environment variable fallback
        connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
    }
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.Error.WriteLine("Connection string 'DefaultConnection' not found. Set it in appsettings.json or as an environment variable.");
    return;
}

builder.Services.AddDbContext<BDQit>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<BDQit>();

Console.WriteLine("Starting database seeding...");
Console.WriteLine("======================================");
// Ensure database schema exists (apply migrations)
Console.WriteLine("Ensuring database is created/migrated...");
try
{
    await context.Database.MigrateAsync();
    Console.WriteLine("Database migrations applied (or database created).");
}
catch (Exception ex)
{
    Console.WriteLine($"Warning: failed to apply migrations: {ex.Message}");
    Console.WriteLine("Attempting EnsureCreated() as a fallback to create tables for the current model.");
    try
    {
        var created = await context.Database.EnsureCreatedAsync();
        Console.WriteLine(created ? "Database created via EnsureCreated()." : "Database already exists (EnsureCreated returned false).");
    }
    catch (Exception ex2)
    {
        Console.WriteLine($"EnsureCreated fallback also failed: {ex2.Message}");
        Console.WriteLine("Proceeding — Clear/Seed may still fail if tables are missing.");
    }
}

try
{
    await ClearDatabase(context);
}
catch (Exception ex)
{
    Console.WriteLine($"Warning while clearing database: {ex.Message}");
    Console.WriteLine("Continuing to seeding — existing tables may not have been present.");
}
await SeedData(context);

Console.WriteLine("======================================");
Console.WriteLine("Database seeding completed!");

async Task ClearDatabase(BDQit context)
{
    Console.WriteLine("Clearing existing data...");

    context.TestResults.RemoveRange(context.TestResults);
    context.AnswerOptions.RemoveRange(context.AnswerOptions);
    context.Question.RemoveRange(context.Question);
    context.Chapter.RemoveRange(context.Chapter);
    context.Materials.RemoveRange(context.Materials);
    context.TeacherClass.RemoveRange(context.TeacherClass);
    context.Users.RemoveRange(context.Users);
    context.Classes.RemoveRange(context.Classes);
    context.Course.RemoveRange(context.Course);
    context.School.RemoveRange(context.School);

    await context.SaveChangesAsync();
    Console.WriteLine("Database cleared.");
}

async Task SeedData(BDQit context)
{
    var random = new Random(42);

    var schools = new List<School>
    {
        new() { NameSchool = "בית ספר יובלים" },
        new() { NameSchool = "תיכון אופק" },
        new() { NameSchool = "מקיף גבעתיים" },
        new() { NameSchool = "בית ספר אופק" },
        new() { NameSchool = "תיכון הרצליה" }
    };
    context.School.AddRange(schools);
    await context.SaveChangesAsync();
    Console.WriteLine($"Added {schools.Count} schools");

    var classNames = new[] { "א'", "ב'", "ג'", "ד'", "ה'", "ו'", "ז'", "ח'", "ט'", "י'", "י\"א", "י\"ב" };
    var classes = new List<Classes>();
    var classCounter = 1;

    foreach (var school in schools)
    {
        var classesPerSchool = random.Next(3, 5);
        for (int i = 0; i < classesPerSchool; i++)
        {
            var gradeNum = random.Next(0, classNames.Length);
            classes.Add(new Classes
            {
                ClassName = $"כיתה {classNames[gradeNum]} - {school.NameSchool.Split(' ').Last()}",
                SchoolId = school.SchoolId
            });
            classCounter++;
        }
    }
    context.Classes.AddRange(classes);
    await context.SaveChangesAsync();
    Console.WriteLine($"Added {classes.Count} classes");

    var hebrewFirstNames = new[]
    {
        "דוד", "שרה", "משה", "רחל", "יעקב", "לאה", "אברהם", "שרה", "יצחק", "רבקה",
        "יוסי", "מיכל", "איתי", "נועה", "גיל", "הילה", "עודד", "שיר", "רועי", "יסמין",
        "אורי", "גאיה", "תמר", "יונתן", "אור", "מאור", "שקד", "ניתאי", "אמיר", "עדן",
        "ליאור", "יערה", "שון", "הדס", "בועז", "קורל", "איתן", "נגה", "עידו", "רותם",
        "שילה", "זיו", "אביב", "מעיין", "טל", "שחר", "רום", "יובל", "שמחה", "חן"
    };

    var hebrewLastNames = new[]
    {
        "כהן", "לוי", "ביטון", "יעקב", "גרינברג", "פרידמן", "שמואלי", "ברק", "דניאל", "אופיר",
        "שפירא", "גולדברג", "רוזנברג", "שלום", "אברהמי", "בן דוד", "זינגר", "ויסמן", "קליין", "מרגלית",
        "נחום", "בר און", "גרינברג", "שריד", "פלד", "אלמוג", "בוטבול", "עמר", "זרטל", "סלומון",
        "חסון", "בן שלום", "גלעד", "רשף", "מזרחי", "פרחי", "שחר", "לביא", "שוב", "גולדשטיין"
    };

    var teachers = new List<Users>();
    for (int i = 0; i < 15; i++)
    {
        var firstName = hebrewFirstNames[random.Next(hebrewFirstNames.Length)];
        var lastName = hebrewLastNames[random.Next(hebrewLastNames.Length)];
        teachers.Add(new Users
        {
            UserName = $"{firstName} {lastName}",
            UserEmail = $"teacher{i}@school.gov.il",
            UserPassword = BCrypt.Net.BCrypt.HashPassword("password123", BCrypt.Net.BCrypt.GenerateSalt(12)),
            Role = "Teacher",
            UserImageUrl = $"https://randomuser.me/api/portraits/{(random.Next(2) == 0 ? "men" : "women")}/{random.Next(1, 50)}.jpg"
        });
    }
    context.Users.AddRange(teachers);
    await context.SaveChangesAsync();
    Console.WriteLine($"Added {teachers.Count} teachers");

    var students = new List<Users>();
    for (int i = 0; i < 200; i++)
    {
        var firstName = hebrewFirstNames[random.Next(hebrewFirstNames.Length)];
        var lastName = hebrewLastNames[random.Next(hebrewLastNames.Length)];
        var classId = classes[random.Next(classes.Count)].ClassId;
        students.Add(new Users
        {
            UserName = $"{firstName} {lastName}",
            UserEmail = $"student{i}@student.school.gov.il",
            UserPassword = BCrypt.Net.BCrypt.HashPassword("password123", BCrypt.Net.BCrypt.GenerateSalt(12)),
            Role = "Student",
            UserImageUrl = $"https://randomuser.me/api/portraits/{(random.Next(2) == 0 ? "men" : "women")}/{random.Next(50, 99)}.jpg",
            ClassId = classId
        });
    }
    context.Users.AddRange(students);
    await context.SaveChangesAsync();
    Console.WriteLine($"Added {students.Count} students");

    var teacherClasses = new List<TeacherClass>();
    foreach (var teacher in teachers)
    {
        var numClasses = random.Next(1, 4);
        var assignedClasses = classes.OrderBy(_ => random.Next()).Take(numClasses).ToList();
        foreach (var cls in assignedClasses)
        {
            teacherClasses.Add(new TeacherClass
            {
                TeacherId = teacher.UserId,
                ClassId = cls.ClassId
            });
        }
    }
    context.TeacherClass.AddRange(teacherClasses);
    await context.SaveChangesAsync();
    Console.WriteLine($"Added {teacherClasses.Count} teacher-class relationships");

    var courseNames = new[]
    {
        "מתמטיקה", "אנגלית", "עברית", "מדעים", "היסטוריה", "גיאוגרפיה", "פיזיקה", "כימיה",
        "ביולוגיה", "ספרות", "אמנות", "מוזיקה", "חנ\"ג", "מחשבים", "צרפתית"
    };

    var courses = new List<Course>();
    // Each class gets one course
    foreach (var cls in classes)
    {
        var courseName = courseNames[random.Next(courseNames.Length)];
        courses.Add(new Course
        {
            CourseName = courseName,
            SchoolId = cls.SchoolId
        });
    }
    context.Course.AddRange(courses);
    await context.SaveChangesAsync();
    Console.WriteLine($"Added {courses.Count} courses");

    var chapters = new List<Chapter>();
    foreach (var course in courses)
    {
        var numChapters = random.Next(3, 6);
        for (int i = 1; i <= numChapters; i++)
        {
            chapters.Add(new Chapter
            {
                Name = $"פרק {i} - {GetChapterName(i)}",
                CourseId = course.CourseId
            });
        }
    }
    context.Chapter.AddRange(chapters);
    await context.SaveChangesAsync();
    Console.WriteLine($"Added {chapters.Count} chapters");

    var questionTemplates = new[]
    {
        "מהו ההגדרה של {0}?",
        "חשב את התוצאה של {0}",
        "מי כתב את {0}?",
        "מתי התרחש {0}?",
        "הסבר את משמעות {0}",
        "מה הקשר בין {0} ל-{1}?",
        "זהה את התשובה הנכונה: {0}",
        "מה הפתרון לתרגיל {0}?",
        "איך פותרים את המשוואה {0}?",
        "מהו עיקרון {0}?"
    };

    var questionTopics = new[]
    {
        "חיבור", "חיסור", "כפל", "חילוק", "שברים", "אחוזים", "משוואות", "זוויות",
        "משולשים", "ריבועים", "מעגלים", "נפחים", "שטחים", "היקף", "מדידות",
        "מילים", "פעלים", "שמות עצם", "תארים", "משפטים", "ניקוד", "אותיות",
        "שירה", "פרוזה", "סיפור קצר", "מאמר", "כתבה", "ידיעה", "מכתב", "הגות",
        "תנ\"ך", "מלחמת העצמאות", "מלחמת ששת הימים", "הקמת המדינה", "העליות",
        "ישראל", "אירופה", "אסיה", "אמריקה", "אפריקה", "אוקיאניה", "הרים", "נהרות",
        "תא תא", "DNA", "RNA", "חלבונים", "אנזימים", "מיטוכונדריה", "גרעין"
    };

    var questions = new List<Question>();
    foreach (var chapter in chapters)
    {
        var numQuestions = random.Next(4, 8);
        for (int i = 0; i < numQuestions; i++)
        {
            var topic1 = questionTopics[random.Next(questionTopics.Length)];
            var topic2 = questionTopics[random.Next(questionTopics.Length)];
            var template = questionTemplates[random.Next(questionTemplates.Length)];
            var questionText = string.Format(template, topic1, topic2);

            questions.Add(new Question
            {
                Questions = questionText,
                Level = random.Next(1, 4),
                ChapterId = chapter.ChapterId
            });
        }
    }
    context.Question.AddRange(questions);
    await context.SaveChangesAsync();
    Console.WriteLine($"Added {questions.Count} questions");

    var answerOptionTemplates = new[]
    {
        "אפשרות א'", "אפשרות ב'", "אפשרות ג'", "אפשרות ד'"
    };

    var correctAnswers = new[]
    {
        "תשובה נכונה א'", "התשובה הנכונה היא ב'", "ג - התשובה הנכונה", "התשובה הנכונה היא ד'",
        "נכון מאוד", "בהחלט נכון", "זו התשובה הנכונה", "התשובה המדויקת",
        "א', זו התשובה", "ב', זו התשובה הנכונה", "ג', תשובה נכונה", "ד', התשובה הנכונה"
    };

    var wrongAnswers = new[]
    {
        "תשובה שגויה א'", "זו תשובה לא נכונה", "לא נכון", "שגוי",
        "ב', זו לא התשובה הנכונה", "התשובה הלא נכונה", "לא הפתרון", "טעות",
        "ג', זו אינה התשובה", "ד', לא נכון", "לא, זה לא נכון", "טעות בחישוב",
        "אינו נכון", "לא מדויק", "שגיאה", "תשובה לא נכונה"
    };

    var answerDescriptions = new[]
    {
        "זוהי התשובה הנכונה מכיוון ש...", "הסבר: התשובה הנכונה היא זו כי...", "נכון! הסיבה לכך היא...",
        "טעות. התשובה הנכונה היא אחרת מכיוון ש...", "לא נכון. יש לשנות את הגישה כי...",
        "שגוי. התשובה הנכונה מבוססת על העיקרון של...", "זו אינה התשובה הנכונה כי...",
        "לא. הפתרון הנכון דורש הבנה של..."
    };

    var answerOptions = new List<AnswerOptions>();
    foreach (var question in questions)
    {
        var correctIndex = random.Next(4);
        for (int i = 0; i < 4; i++)
        {
            var isCorrect = i == correctIndex;
            var optionText = isCorrect
                ? correctAnswers[random.Next(correctAnswers.Length)]
                : wrongAnswers[random.Next(wrongAnswers.Length)];
            
            var description = answerDescriptions[random.Next(answerDescriptions.Length)];

            answerOptions.Add(new AnswerOptions
            {
                QuestionId = question.QuestionId,
                Option = answerOptionTemplates[i],
                IsCorrect = isCorrect,
                Description = description
            });
        }
    }
    context.AnswerOptions.AddRange(answerOptions);
    await context.SaveChangesAsync();
    Console.WriteLine($"Added {answerOptions.Count} answer options");

    var materialNames = new[]
    {
        "סיכום הפרק", "מצגת השיעור", "דף עבודה", "סרטון הסבר", "מפרט חומר",
        "דוגמאות לתרגילים", "מדריך למורה", "תרגילי חיזוק", "בחינה לדוגמה",
        "טבלאות וגרפים", "מפות מושגים", "רשימת מילים", "קטעי קריאה"
    };

    var materials = new List<Materials>();
    foreach (var course in courses)
    {
        var numMaterials = random.Next(2, 5);
        for (int i = 0; i < numMaterials; i++)
        {
            materials.Add(new Materials
            {
                MatName = $"{materialNames[random.Next(materialNames.Length)]} - {course.CourseName}",
                MatDescription = $"חומר לימוד בנושא {course.CourseName}. מיועד לתלמידי הקורס.",
                MatLink = $"https://example.com/materials/{course.CourseId}/{i + 1}",
                CourseId = course.CourseId
            });
        }
    }
    context.Materials.AddRange(materials);
    await context.SaveChangesAsync();
    Console.WriteLine($"Added {materials.Count} materials");

    var subjects = courseNames.ToList();
    var testTitles = new[]
    {
        "בוחן חישוב", "מבחן מסכם", "בחינת ביניים", "מבחן סוף שנה", "בוחן קצר",
        "מבחן מעשי", "בוחן תיאורטי", "מבחן הבנה", "בוחן ידע", "מבחן מחוון"
    };

    var testResults = new List<TestResult>();
    foreach (var student in students)
    {
        var numTests = random.Next(1, 5);
        for (int i = 0; i < numTests; i++)
        {
            var subject = subjects[random.Next(subjects.Count)];
            var maxScore = random.Next(50, 101);
            var score = random.Next(30, maxScore + 1);
            var daysAgo = random.Next(1, 180);

            testResults.Add(new TestResult
            {
                StudentId = student.UserId,
                Subject = subject,
                Title = $"{testTitles[random.Next(testTitles.Length)]} - {subject}",
                Date = DateTime.Now.AddDays(-daysAgo),
                Score = score,
                MaxScore = maxScore,
                Duration = random.Next(15, 91)
            });
        }
    }
    try
    {
        context.TestResults.AddRange(testResults);
        await context.SaveChangesAsync();
        Console.WriteLine($"Added {testResults.Count} test results");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: failed to save TestResults: {ex.Message}");
        Console.WriteLine("Continuing — test results not saved.");
    }

    Console.WriteLine("\n=== Final Data Summary ===");
    Console.WriteLine($"Schools: {context.School.Count()}");
    Console.WriteLine($"Classes: {context.Classes.Count()}");
    Console.WriteLine($"Teachers: {context.Users.Count(u => u.Role == "Teacher")}");
    Console.WriteLine($"Students: {context.Users.Count(u => u.Role == "Student")}");
    Console.WriteLine($"Teacher-Class links: {context.TeacherClass.Count()}");
    Console.WriteLine($"Courses: {context.Course.Count()}");
    Console.WriteLine($"Chapters: {context.Chapter.Count()}");
    Console.WriteLine($"Questions: {context.Question.Count()}");
    Console.WriteLine($"Answer Options: {context.AnswerOptions.Count()}");
    Console.WriteLine($"Materials: {context.Materials.Count()}");
    Console.WriteLine($"Test Results: {context.TestResults.Count()}");

    // Fix any plaintext passwords: hash them with BCrypt so login will work
    try
    {
        var usersToFix = context.Users.Where(u => u.UserPassword != null && !u.UserPassword.StartsWith("$2")).ToList();
        Console.WriteLine($"Found {usersToFix.Count} users with non-hashed passwords. Hashing now...");
        foreach (var u in usersToFix)
        {
            // original plaintext is unknown; assume default seed password 'password123'
            u.UserPassword = BCrypt.Net.BCrypt.HashPassword("password123", BCrypt.Net.BCrypt.GenerateSalt(12));
        }
        await context.SaveChangesAsync();
        Console.WriteLine($"Hashed passwords for {usersToFix.Count} users.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: failed to hash existing passwords: {ex.Message}");
    }
}

string GetChapterName(int num)
{
    var chapterNames = new[]
    {
        "מבוא", "בסיס תיאוריה", "עקרונות יסוד", "שיטות ודרכים",
        "דוגמאות מעשיות", "תרגילים", "סיכום", "הרחבה"
    };
    return num <= chapterNames.Length ? chapterNames[num - 1] : $"נושא מתקדם {num - 7}";
}
