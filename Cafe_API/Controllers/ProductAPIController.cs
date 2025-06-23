using AutoMapper;
using Cafe_API.Models;
using Cafe_API.Models.Dto;
using Cafe_API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Cafe_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public ProductAPIController(IProductRepository productRepo, IMapper mapper, ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _response = new APIResponse();
            _categoryRepo = categoryRepo;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetProducts()
        {
            var products = await _productRepo.GetAllAsync();
            _response.Result = products;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name ="GetProduct")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetProduct(int id)
        {
            if (id == 0)
            {
                _response.IsSuccess = false;
                _response.Errors.Add("No id of 0 exists.");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var product = await _productRepo.GetAsync(u => u.Id == id);
            if (product == null)
            {
                _response.IsSuccess = false;
                _response.Errors.Add("No id of " + id + " exists.");
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.Result = product;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] ProductCreateDTO productDto)
        {
            if (productDto == null)
            {
                _response.IsSuccess = false;
                _response.Errors.Add("Product must be valid and id must be 0");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            if(await _categoryRepo.GetAsync(u=>u.Id == productDto.CategoryId) == null)
            {
                _response.IsSuccess = false;
                _response.Errors.Add("CategoryId of " + productDto.CategoryId + " does not exist.");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var product = _mapper.Map<Product>(productDto);
            await _productRepo.CreateAsync(_mapper.Map<Product>(product));
            _response.StatusCode = HttpStatusCode.Created;
            _response.Result = product;
            return CreatedAtRoute("GetProduct", new { id = product.Id }, _response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> UpdateProduct(int id, [FromBody]ProductUpdateDTO productDto)
        {
            if (id == 0 || id != productDto.Id)
            {
                _response.IsSuccess = false;
                _response.Errors.Add("Id must not be 0 and must match");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            if (await _categoryRepo.GetAsync(u=>u.Id == productDto.CategoryId) == null)
            {
                _response.IsSuccess = false;
                _response.Errors.Add("Invalid Category");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var product = await _productRepo.GetAsync(u=>u.Id==productDto.Id/*, tracked:false*/);
            if (product == null)
            {
                _response.IsSuccess = false;
                _response.Errors.Add("Product with id of " + id + " does not exist.");
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound();
            }
            //product = _mapper.Map<Product>(productDto);
            _mapper.Map(productDto,product);    // this approach avoids tracking errors..

            await _productRepo.UpdateAsync(product);

            _response.Result = productDto;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int id)
        {
            if (id == 0)
            {
                _response.IsSuccess = false;
                _response.Errors.Add("Id of 0 does not exist.");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var product = await _productRepo.GetAsync(u => u.Id == id);
            if (product == null)
            {
                _response.IsSuccess = false;
                _response.Errors.Add("No id of " + id + " exists.");
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _productRepo.RemoveAsync(product);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.Result = product;
            return NoContent();
        }
    }
}
