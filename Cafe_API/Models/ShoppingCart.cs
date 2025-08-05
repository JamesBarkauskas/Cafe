using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe_API.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        // 2 FK - what product, what user..
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public AppUser User { get; set; }

        // create an unmapped property for price to display price in UI, NotMapped won't create db column..
        [NotMapped]
        public double Price { get; set; }
    }
}

