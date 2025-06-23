using System.ComponentModel.DataAnnotations;

namespace Cafe_Web.Models.Dto
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
