using Cafe_API.Data;
using Cafe_API.Models;
using Cafe_API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Cafe_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CategoryAPIController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;
        protected APIResponse _response;
        public CategoryAPIController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
            _response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetCategories()
        {

            var categories = await _categoryRepo.GetAllAsync();
            _response.Result = categories;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategory(int id)
        {
            if (id == 0)
            {
                _response.IsSuccess = false;
                _response.Errors.Add("No id of 0 exists.");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            //var category = _db.Categories.FirstOrDefault(u => u.Id == id);  Find() is better here, more efficient..
            // Find() searches by PK, still returns null if not found (unlike First())
            var category = await _categoryRepo.GetAsync(u => u.Id == id);
            if (category == null)
            {
                _response.IsSuccess = false;
                _response.Errors.Add("No id of " + id + " exists.");
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.Result = category;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] Category category)
        {
            if (category == null || category.Id != 0)
            {
                _response.IsSuccess = false;
                _response.Errors.Add("Category must be valid and id must be 0");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            await _categoryRepo.CreateAsync(category);
            _response.StatusCode = HttpStatusCode.Created;
            _response.Result = category;
            return CreatedAtRoute("GetCategory", new { id = category.Id }, _response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> UpdateCategory(int id, [FromBody] Category category)
        {
            if (id == 0 || id != category.Id) 
            {
                _response.IsSuccess = false;
                _response.Errors.Add("Id must not be 0 and must match");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response); 
            }
            var cat = await _categoryRepo.GetAsync(u=>u.Id == id);
            if (cat == null) 
            {
                _response.IsSuccess = false;
                _response.Errors.Add("Category with id of " + id + " does not exist.");
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(); 
            }

            cat.Name = category.Name;
            await _categoryRepo.UpdateAsync(cat);

            //_response.Result = cat;
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<APIResponse>> DeleteCategory(int id)
        {
            if (id == 0) 
            {
                _response.IsSuccess = false;
                _response.Errors.Add("Id of 0 does not exist.");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response); 
            }
            var category = await _categoryRepo.GetAsync(u => u.Id == id);
            if (category == null) 
            {
                _response.IsSuccess = false;
                _response.Errors.Add("No id of " + id + " exists.");
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response); 
            }

            await _categoryRepo.RemoveAsync(category);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.Result = category;
            return Ok(_response);
        }
    }
}
