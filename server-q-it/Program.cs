using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Entities;
using Repository.interfaces;
using Repository.Repositories;
using Service.Dto;
using Service.Interface;
using Service.Services;
using System.Text;
using webApiProject.Middleware;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 100_000_000; // 100MB
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100_000_000; // 100MB
});

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Q-it API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddHttpClient("QuizClient", client =>
{
    client.Timeout = TimeSpan.FromMinutes(5);
});
builder.Services.AddHttpClient();
builder.Services.AddDbContext<BDQit>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IContext>(sp => sp.GetRequiredService<BDQit>());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IRepository<School>, SchoolRepository>();
builder.Services.AddScoped<SchoolService>();
builder.Services.AddScoped<IService<School>>(sp => sp.GetRequiredService<SchoolService>());

builder.Services.AddScoped<IRepository<Course>, CourseRepository>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<IService<Course>>(sp => sp.GetRequiredService<CourseService>());
builder.Services.AddScoped<ICourseActions>(sp => sp.GetRequiredService<CourseService>());

builder.Services.AddScoped<IRepository<Materials>, MaterialsRepository>();
builder.Services.AddScoped<MaterialsService>();
builder.Services.AddScoped<IService<Materials>>(sp => sp.GetRequiredService<MaterialsService>());

builder.Services.AddScoped<IRepository<Users>, UserRepository>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<IService<Users>>(sp => sp.GetRequiredService<UsersService>());
builder.Services.AddScoped<IUserActions>(sp => sp.GetRequiredService<UsersService>());

builder.Services.AddScoped<UserLoginService>();
builder.Services.AddScoped<ILogin>(sp => sp.GetRequiredService<UserLoginService>());
builder.Services.AddScoped<IAuthActions>(sp => sp.GetRequiredService<UserLoginService>());

builder.Services.AddScoped<IRepository<Classes>, ClassRepository>();
builder.Services.AddScoped<ClassService>();
builder.Services.AddScoped<IService<Classes>>(sp => sp.GetRequiredService<ClassService>());
builder.Services.AddScoped<IClassActions>(sp => sp.GetRequiredService<ClassService>());

builder.Services.AddScoped<IRepository<Chapter>, ChapterRepository>();
builder.Services.AddScoped<ChapterService>();
builder.Services.AddScoped<IService<Chapter>>(sp => sp.GetRequiredService<ChapterService>());
builder.Services.AddScoped<IChapterActions>(sp => sp.GetRequiredService<ChapterService>());

builder.Services.AddScoped<IRepository<Question>, QuestionRepository>();
builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<IService<Question>>(sp => sp.GetRequiredService<QuestionService>());

builder.Services.AddScoped<IRepository<AnswerOptions>, AnswerOptionRepository>();
builder.Services.AddScoped<AnswerOptionsService>();
builder.Services.AddScoped<IService<AnswerOptions>>(sp => sp.GetRequiredService<AnswerOptionsService>());

builder.Services.AddScoped<IRepository<TeacherClass>, TeacherClassRepository>();
builder.Services.AddScoped<TeacherClassService>();
builder.Services.AddScoped<IService<TeacherClass>>(sp => sp.GetRequiredService<TeacherClassService>());

builder.Services.AddScoped<IStatsService, StatsService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<ITestTakingActions, TestTakingService>();

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "q-it-api";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "q-it-client";

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync($"Error: {ex.Message}");
    }
});

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
