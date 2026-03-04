using Microsoft.AspNetCore.Http;

namespace Service.Dto
{
    public class MaterialsDto
    {
        public required string MatName { get; set; }
        public string? MatDescription { get; set; }
        public int CourseId { get; set; }
        public string? MatLink { get; set; }
        public IFormFile? FileMaterial { get; set; }
    }
}
