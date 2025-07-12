using Cafe_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cafe_Web.Models.VM
{
    public class ProductUpdateVM
    {
        public ProductUpdateDTO Product { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
    }
}
