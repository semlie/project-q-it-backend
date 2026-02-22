using CodeFirst.Models;
using Repository.Entities;
using Repository.interfaces;
using Repository.Repositories;
using Service.Dto;
using Service.Interface;
using Service.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Register DbContext
//builder.Services.AddDbContext<BDQit>(options =>
   // options.UseSqlite("Data Source=../codefirst/database.db"));
builder.Services.AddScoped<IContext, BDQit>();
// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Register repositories and services
builder.Services.AddScoped<IRepository<School>, SchoolRepository>();
builder.Services.AddScoped<IService<School>, SchoolService>();
builder.Services.AddScoped<IRepository<Course>, CourseRepository>();
builder.Services.AddScoped<IService<Course>, CourseService>();
builder.Services.AddScoped<IRepository<Users>, UserRepository>();
builder.Services.AddScoped<IService<Users>, UsersService>();
builder.Services.AddScoped<ILogin, UserLoginService>();
builder.Services.AddScoped<IRepository<Materials>, MaterialsRepository>();
builder.Services.AddScoped<IService<Materials>, MaterialsService>();
builder.Services.AddScoped<IRepository<Question>, QuestionRepository>();
builder.Services.AddScoped<IService<Question>, QuestionService>();
builder.Services.AddScoped<IRepository<AnswerOptions>, AnswerOptionRepository>();
builder.Services.AddScoped<IService<AnswerOptions>, AnswerOptionsService>();
builder.Services.AddScoped<IRepository<Chapter>, ChapterRepository>();
builder.Services.AddScoped<IService<Chapter>, ChapterService>();
// Add CORS
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
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
