using System.ComponentModel.DataAnnotations;

namespace Cafe_Web.Models.Dto
{
    public class ProductCreateDTO   // dto including only fields the user should provide.. no id since that's provided by db
    {
        
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int? Calories { get; set; }
        public string? ImageUrl { get; set; }
        [Display(Name="Category")]
        public int CategoryId { get; set; }
    }
}
