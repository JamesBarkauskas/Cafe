namespace Cafe_API.Models.Dto
{
    public class ProductUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int? Calories { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
    }
}
