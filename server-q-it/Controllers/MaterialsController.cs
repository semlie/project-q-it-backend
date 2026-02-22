using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IService<Materials> service;
        
        public MaterialsController(IService<Materials> service)
        {
            this.service = service;
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

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Materials value)
        {
            var material = service.GetById(id);
            if (material == null)
            {
                return NotFound();
            }
            service.UpdateItem(id, value);
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
            service.DeleteItem(id);
            return NoContent();
        }
    }
}
