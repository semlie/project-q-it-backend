using Microsoft.AspNetCore.Hosting;
using Repository.Entities;
using Repository.interfaces;
using Service.Dto;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UsersService : IService<Users>, IUserActions
    {
        private readonly IRepository<Users> _repository;
        private readonly IContext _context;
        private readonly IWebHostEnvironment _env;

        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        public UsersService(IRepository<Users> repository, IContext context, IWebHostEnvironment env)
        {
            _repository = repository;
            _context = context;
            _env = env;
        }

        public async Task<List<Users>> getAllAsync()
        {
            return await _repository.getAllAsync();
        }

        public async Task<Users> getByIdAsync(int id)
        {
            return await _repository.getByIdAsync(id);
        }

        public async Task<Users> addItemAsync(Users item)
        {
            return await _repository.AddAsync(item);
        }

        public async Task updateItemAsync(int id, Users item)
        {
            await _repository.UpdateAsync(item);
        }

        public async Task deleteItemAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<Course>> GetUserCoursesAsync(int userId)
        {
            var user = await _repository.getByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException($"User with ID {userId} not found");

            if (user.Role == "Student")
            {
                if (user.ClassId.HasValue)
                {
                    var studentClass = _context.Set<Classes>().FirstOrDefault(c => c.ClassId == user.ClassId.Value);
                    if (studentClass != null)
                    {
                        return _context.Set<Course>().Where(c => c.SchoolId == studentClass.SchoolId).ToList();
                    }
                    return new List<Course>();
                }
                return new List<Course>();
            }
            else
            {
                var teacherClasses = _context.Set<TeacherClass>().Where(tc => tc.TeacherId == user.UserId).ToList();

                if (teacherClasses == null || !teacherClasses.Any())
                    return new List<Course>();

                var classIds = teacherClasses.Select(tc => tc.ClassId).ToList();
                var classSchoolIds = _context.Set<Classes>()
                    .Where(c => classIds.Contains(c.ClassId))
                    .Select(c => c.SchoolId)
                    .Distinct()
                    .ToList();

                var courses = _context.Set<Course>()
                    .Where(c => classSchoolIds.Contains(c.SchoolId))
                    .ToList();

                return courses;
            }
        }

        public async Task DeleteUserImageAsync(int id)
        {
            var user = await _repository.getByIdAsync(id);
            if (user == null)
                throw new InvalidOperationException($"User with ID {id} not found");

            if (!string.IsNullOrWhiteSpace(user.UserImageUrl))
            {
                var relativePath = user.UserImageUrl.Replace('/', Path.DirectorySeparatorChar);
                var fileFullPath = Path.Combine(_env.ContentRootPath, relativePath);
                var imagesDirectory = Path.Combine(_env.ContentRootPath, "images");

                if (Path.GetFullPath(fileFullPath).StartsWith(Path.GetFullPath(imagesDirectory), StringComparison.OrdinalIgnoreCase)
                    && System.IO.File.Exists(fileFullPath))
                {
                    System.IO.File.Delete(fileFullPath);
                }
            }

            user.UserImageUrl = string.Empty;
            await _repository.UpdateAsync(user);
        }

        public async Task<Users> CreateUserAsync(UsersDto userDto)
        {
            var imagePath = string.Empty;

            if (userDto.FileImage is not null && userDto.FileImage.Length > 0)
            {
                var fileExtension = Path.GetExtension(userDto.FileImage.FileName).ToLower();
                if (!AllowedImageExtensions.Contains(fileExtension))
                    throw new InvalidOperationException("Only image files (jpg, jpeg, png, gif) are allowed");

                var imagesDirectory = Path.Combine(_env.ContentRootPath, "images");
                Directory.CreateDirectory(imagesDirectory);

                var newFileName = $"{Guid.NewGuid()}{fileExtension}";
                var fileFullPath = Path.Combine(imagesDirectory, newFileName);

                await using var stream = new FileStream(fileFullPath, FileMode.Create);
                await userDto.FileImage.CopyToAsync(stream);

                imagePath = Path.Combine("images", newFileName).Replace("\\", "/");
            }

            var hashedPassword = UserLoginService.HashPassword(userDto.UserPassword);
            var user = new Users
            {
                UserPassword = hashedPassword,
                UserName = userDto.UserName,
                UserEmail = userDto.UserEmail,
                Role = userDto.Role,
                ClassId = userDto.ClassId,
                UserImageUrl = imagePath
            };

            return await _repository.AddAsync(user);
        }
    }
}
