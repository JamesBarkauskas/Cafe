using Cafe_API.Data;
using Cafe_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cafe_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CategoryAPIController(AppDbContext db) 
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetCategories()
        {
            return Ok(_db.Categories);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCategory(int id)
        {
            if (id==0)
            {
                return BadRequest();
            }
            //var category = _db.Categories.FirstOrDefault(u => u.Id == id);  Find() is better here, more efficient..
            // Find() searches by PK, still returns null if not found (unlike First())
            var category = _db.Categories.Find(id);
            if (category == null) { return NotFound(); }
            return Ok(category);
        }
    }
}
