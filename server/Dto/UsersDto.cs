using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Repository.Entities;

namespace Service.Dto
{
    public class UsersDto
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 100 characters")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string UserPassword { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string UserEmail { get; set; }
        
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
        
        public int? ClassId { get; set; }
        public List<int>? ClassIds { get; set; }
        
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".gif" }, ErrorMessage = "Only image files (jpg, jpeg, png, gif) are allowed")]
        public IFormFile? FileImage { get; set; }
    }
    
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }
        
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
