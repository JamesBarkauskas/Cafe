using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe_API.Models
{
    public class ShoppingCart
    {
        // old approach - using just a ShoppingCart model rather than including individual CartItem ...
        //public int Id { get; set; }

        //// 2 FK - what product, what user..
        //public int ProductId { get; set; }
        //[ForeignKey("ProductId")]
        //[ValidateNever]
        //public Product Product { get; set; }

        //public string UserId { get; set; }
        //[ForeignKey("UserId")]
        //[ValidateNever]
        //public AppUser User { get; set; }

        //// create an unmapped property for price to display price in UI, NotMapped won't create db column..
        //[NotMapped]
        //public double Price { get; set; }

        // new approach, shoppingCart represents the whole cart
        public int Id { get; set; }

        [Required]  // UserId cannot be null.. a cart *must* be tied to a User - EF enforces this at db level
        public string UserId { get; set; }  // fk to the appUser table, tells efCore which user this cart belongs to
        public AppUser User { get; set; }   // nav property to user, lets us access user's info.. 

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();    // nav property for items in the cart
            // by instaniating the cart with 'new list<cartItem>', this makes sure it's not null when we crete a new cart...

        

    }
}

