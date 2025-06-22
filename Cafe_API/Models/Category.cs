using System.ComponentModel.DataAnnotations;

namespace Cafe_API.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
