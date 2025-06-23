using System.ComponentModel.DataAnnotations;

namespace Cafe_Web.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
