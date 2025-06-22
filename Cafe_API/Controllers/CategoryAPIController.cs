using Cafe_API.Data;
using Cafe_API.Models;
using Cafe_API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cafe_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryAPIController(ICategoryRepository categoryRepo) 
        {
            _categoryRepo = categoryRepo;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}", Name ="GetCategory")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategory(int id)
        {
            if (id==0)
            {
                return BadRequest();
            }
            //var category = _db.Categories.FirstOrDefault(u => u.Id == id);  Find() is better here, more efficient..
            // Find() searches by PK, still returns null if not found (unlike First())
            var category = await _categoryRepo.GetAsync(u => u.Id == id);
            if (category == null) { return NotFound(); }
            return Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            if (category == null || category.Id != 0) { return BadRequest(); }
            await _categoryRepo.CreateAsync(category);
            return CreatedAtRoute("GetCategory", new { id = category.Id }, category);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (id == 0 || id != category.Id) { return BadRequest(); }
            var cat = await _categoryRepo.GetAsync(u=>u.Id == id);
            if (cat == null) { return NotFound(); }

            cat.Name = category.Name;

            await _categoryRepo.UpdateAsync(cat);
            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id == 0) { return BadRequest(); }
            var category = await _categoryRepo.GetAsync(u => u.Id == id);
            if (category == null) { return NotFound(); }
            await _categoryRepo.RemoveAsync(category);
            return Ok();
        }
    }
}
