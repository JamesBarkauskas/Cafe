using System.ComponentModel.DataAnnotations;

namespace Cafe_Web.Models.Dto
{
    public class CategoryCreateDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
