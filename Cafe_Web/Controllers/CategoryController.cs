using AutoMapper;
using Cafe_Web.Models;
using Cafe_Web.Models.Dto;
using Cafe_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Cafe_Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> categoryList = new();
            var response = await _categoryService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                categoryList = JsonConvert.DeserializeObject<List<Category>>(Convert.ToString(response.Result));
            }

            return View(categoryList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryCreateDTO category)
        {
            if (ModelState.IsValid)
            {
                var response = await _categoryService.CreateAsync<APIResponse>(category);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Category created";
                    return RedirectToAction(nameof(Index));
                }
            }
            //TempData["error"] = "Error encountered";
            return View(category);
        }


    }
}
