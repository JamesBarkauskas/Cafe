using AutoMapper;
using Cafe_Web.Models;
using Cafe_Web.Models.Dto;
using Cafe_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace Cafe_Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> productList = new();
            var response = await _productService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<Product>>(Convert.ToString(response.Result));
            }

            return View(productList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateAsync<APIResponse>(dto);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created";
                    return RedirectToAction("Index");
                }
                TempData["error"] = response.Errors.First();
             
            }
            
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var response = await _productService.GetAsync<APIResponse>(id);
            if (response != null && response.IsSuccess)
            {
                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<ProductUpdateDTO>(model));
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateDTO dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateAsync<APIResponse>(dto);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product updated";
                    return RedirectToAction("Index");
                }
                TempData["error"] = response.Errors.First();
            }
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.GetAsync<APIResponse>(id);
            if (response != null && response.IsSuccess)
            {
                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductDTO dto)
        {
            var response = await _productService.DeleteAsync<APIResponse>(dto.Id);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error encountered";
            return View(dto);
        }
    }
}
