using System.ComponentModel.DataAnnotations;

namespace Cafe_API.Models
{
    public class CartItem
    {
        // represents a product in the cart, including quantity
        public int Id { get; set; }

        [Required]
        public int ShoppingCartId { get; set; } // fk that connects each cartItem to a shoppingCart..
        public ShoppingCart ShoppingCart { get; set; }// nav propety, allows access to the full shoppingCart based on the Fk above..

        [Required]
        public int ProductId { get; set; }  // fk to Product.. represents which Product is being added
        public Product Product { get; set; }// nav prop.. allows access to whole Product obj, like name, price..

        public int Quantity { get; set; }
    }
}
