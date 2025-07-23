using Cafe_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cafe_Web.Models.VM
{
    public class ProductUpdateVM
    {
        public ProductUpdateDTO Product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
    }
}
