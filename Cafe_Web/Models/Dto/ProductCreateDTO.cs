namespace Cafe_Web.Models.Dto
{
    public class ProductCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Calories { get; set; }
        public int CategoryId { get; set; }
    }
}
