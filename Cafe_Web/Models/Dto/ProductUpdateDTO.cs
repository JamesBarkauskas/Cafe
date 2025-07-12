using System.ComponentModel.DataAnnotations;

namespace Cafe_Web.Models.Dto
{
    public class ProductUpdateDTO   // includes id, nullable properties common here to suport partial update...
                                    // common to just copy the CreateDTO and add the Id field..
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int? Calories { get; set; }
        [Display(Name="Category")]
        public int CategoryId { get; set; }
    }
}
