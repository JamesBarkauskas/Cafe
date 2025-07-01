using AutoMapper;
using Cafe_Web.Models;
using Cafe_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    }
}
