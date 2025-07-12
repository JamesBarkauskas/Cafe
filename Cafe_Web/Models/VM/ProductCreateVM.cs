using Cafe_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Cafe_Web.Models.VM
{
    public class ProductCreateVM
    {
        public ProductCreateDTO Product { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
    }
}
