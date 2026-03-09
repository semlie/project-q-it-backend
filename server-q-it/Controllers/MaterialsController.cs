using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Repository.Entities;
using Service.Dto;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialsController : ControllerBase
    {
        private readonly IService<Materials> service;
        private readonly IWebHostEnvironment env;
        
        public MaterialsController(IService<Materials> service, IWebHostEnvironment env)
        {
            this.service = service;
            this.env = env;
        }
        [HttpGet("course/{courseId}")]
        public ActionResult<List<Materials>> GetByCourseId(int courseId)
        {
            var materials = service.GetAll().Where(m => m.CourseId == courseId).ToList();
            if (materials == null || materials.Count == 0)
            {
                return NotFound();
            }
            return Ok(materials);
        }
        [HttpGet]
        public ActionResult<List<Materials>> Get()
        {
            var materials = service.GetAll();
            if (materials == null || materials.Count == 0)
            {
                return NotFound();
            }
            return Ok(materials);
        }

        [HttpGet("{id}")]
        public ActionResult<Materials> Get(int id)
        {
            var material = service.GetById(id);
            if (material == null)
            {
                return NotFound();
            }
            return Ok(material);
        }

        [HttpPost]
        public async Task<ActionResult<Materials>> Post([FromForm] MaterialsDto value)
        {
            var materialLink = value.MatLink ?? string.Empty;

            if (value.FileMaterial is not null && value.FileMaterial.Length > 0)
            {
                var materialsDirectory = Path.Combine(env.ContentRootPath, "materials");
                Directory.CreateDirectory(materialsDirectory);

                var fileExtension = Path.GetExtension(value.FileMaterial.FileName);
                var newFileName = $"{Guid.NewGuid()}{fileExtension}";
                var fileFullPath = Path.Combine(materialsDirectory, newFileName);

                await using var stream = new FileStream(fileFullPath, FileMode.Create);
                await value.FileMaterial.CopyToAsync(stream);

                materialLink = Path.Combine("materials", newFileName).Replace("\\", "/");
            }

            var material = new Materials
            {
                MatName = value.MatName,
                MatDescription = value.MatDescription,
                MatLink = materialLink,
                CourseId = value.CourseId
            };

            var createdMaterial = service.AddItem(material);
            return CreatedAtAction(nameof(Get), new { id = createdMaterial.MatId }, createdMaterial);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] MaterialsDto value)
        {
            var material = service.GetById(id);
            if (material == null)
            {
                return NotFound();
            }

            var updatedLink = material.MatLink;

            if (!string.IsNullOrWhiteSpace(value.MatLink))
            {
                updatedLink = value.MatLink;
            }

            if (value.FileMaterial is not null && value.FileMaterial.Length > 0)
            {
                var materialsDirectory = Path.Combine(env.ContentRootPath, "materials");
                Directory.CreateDirectory(materialsDirectory);

                var fileExtension = Path.GetExtension(value.FileMaterial.FileName);
                var newFileName = $"{Guid.NewGuid()}{fileExtension}";
                var fileFullPath = Path.Combine(materialsDirectory, newFileName);

                await using var stream = new FileStream(fileFullPath, FileMode.Create);
                await value.FileMaterial.CopyToAsync(stream);

                TryDeleteMaterialFile(material.MatLink);
                updatedLink = Path.Combine("materials", newFileName).Replace("\\", "/");
            }

            var updatedMaterial = new Materials
            {
                MatId = id,
                MatName = value.MatName,
                MatDescription = value.MatDescription,
                MatLink = updatedLink,
                CourseId = value.CourseId
            };

            service.UpdateItem(id, updatedMaterial);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var material = service.GetById(id);
            if (material == null)
            {
                return NotFound();
            }

            TryDeleteMaterialFile(material.MatLink);
            service.DeleteItem(id);
            return NoContent();
        }

        [HttpGet("{id}/download")]
        public IActionResult Download(int id)
        {
            var material = service.GetById(id);
            if (material == null || string.IsNullOrWhiteSpace(material.MatLink))
            {
                return NotFound();
            }

            if (!TryResolveMaterialFilePath(material.MatLink, out var fullFilePath))
            {
                return NotFound();
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fullFilePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var stream = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var downloadName = Path.GetFileName(fullFilePath);
            return File(stream, contentType, downloadName);
        }

        private void TryDeleteMaterialFile(string? materialLink)
        {
            if (!TryResolveMaterialFilePath(materialLink, out var fullFilePath))
            {
                return;
            }

            if (System.IO.File.Exists(fullFilePath))
            {
                System.IO.File.Delete(fullFilePath);
            }
        }

        private bool TryResolveMaterialFilePath(string? materialLink, out string fullFilePath)
        {
            fullFilePath = string.Empty;
            if (string.IsNullOrWhiteSpace(materialLink))
            {
                return false;
            }

            var relativePath = materialLink.Replace('/', Path.DirectorySeparatorChar);
            var candidatePath = Path.Combine(env.ContentRootPath, relativePath);
            var materialsDirectory = Path.Combine(env.ContentRootPath, "materials");
            var fullMaterialsDirectory = Path.GetFullPath(materialsDirectory);
            var normalizedCandidatePath = Path.GetFullPath(candidatePath);

            if (!normalizedCandidatePath.StartsWith(fullMaterialsDirectory, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            fullFilePath = normalizedCandidatePath;
            return true;
        }
    }
}
