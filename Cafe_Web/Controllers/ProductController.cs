using AutoMapper;
using Cafe_Utility;
using Cafe_Web.Models;
using Cafe_Web.Models.Dto;
using Cafe_Web.Models.VM;
using Cafe_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;

namespace Cafe_Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductService productService, IMapper mapper, ICategoryService categoryService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _mapper = mapper;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> productList = new();
            var response = await _productService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<Product>>(Convert.ToString(response.Result));
            }

            return View(productList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ProductCreateVM productVM = new ProductCreateVM();
            var response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                productVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(
                    Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                APIResponse _response = new();
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                _response.IsSuccess = false;
                _response.Errors = errors;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (ModelState.IsValid)
            {
                var response = await _productService.CreateAsync<APIResponse>(model.Product, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created";
                    return RedirectToAction("Index");
                }
                TempData["error"] = response.Errors.First();
             
            }
            // if model not valid, repopulate the dropdown
            var res = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (res != null && res.IsSuccess)
            {
                model.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(
                    res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            ProductUpdateVM productUpdateVM = new();

            var response = await _productService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ProductUpdateDTO model = JsonConvert.DeserializeObject<ProductUpdateDTO>(Convert.ToString(response.Result));
                productUpdateVM.Product = model;
            }

            // populate dropdown
            response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                productUpdateVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(
                    response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(productUpdateVM);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(ProductUpdateVM model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                // set up image 
                string wwwRootPath = _webHostEnvironment.WebRootPath;   // path to wwwroot folder
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string imgPath = Path.Combine(wwwRootPath, @"images/product");

                    if (!string.IsNullOrEmpty(model.Product.ImageUrl))
                    {
                        // img already exists and udpate to new img..
                        var oldImagePath = Path.Combine(wwwRootPath, model.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(imgPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    model.Product.ImageUrl = @"\images\product\" + fileName;
                }

                // file is null
                // grab the imgUrl if it exists..


                var response = await _productService.UpdateAsync<APIResponse>(model.Product, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product updated";
                    return RedirectToAction("Index");
                }
                TempData["error"] = response.Errors.First();
            }
            var res = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (res != null && res.IsSuccess)
            {
                model.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(
                    res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            ProductDeleteVM productDeleteVM = new();
            var response = await _productService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                productDeleteVM.Product = model;
            }
            response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                productDeleteVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(
                    Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(productDeleteVM);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(ProductDeleteVM model)
        {
            var response = await _productService.DeleteAsync<APIResponse>(model.Product.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error encountered";
            return View(model);
        }
    }
}
