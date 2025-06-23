using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe_Web.Models.Dto
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Calories { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }  // nav property
    }
}
