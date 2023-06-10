using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShopHai.Models;

namespace ShopHai.Models
{
    public class CartDAO
    {
        private readonly ShopClothesContext _ctx;

        public CartDAO(ShopClothesContext ctx)
        {
            _ctx = ctx;
        }

        public void AddToCart(Product product, int quantity, int userId)
        {
            // Find the existing cart item for the user and product
            var cartItem = _ctx.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == product.ProductId);

            if (cartItem == null)
            {
                // Create a new cart item
                cartItem = new Cart
                {
                    UserId = userId,
                    ProductId = product.ProductId,
                    Quantity = quantity,
                    Price = (decimal?)product.Price,
                    Total = (decimal?)(quantity * product.Price)
                };

                // Add the cart item to the database
                _ctx.Carts.Add(cartItem);
            }
            else
            {
                // Update the existing cart item
                cartItem.Quantity += quantity;
                cartItem.Total = (decimal?)(cartItem.Quantity * product.Price);
            }

            // Save changes to the database
            _ctx.SaveChanges();
        }
        public List<Cart> GetCartItemsByUserId(int userId)
        {
            return _ctx.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToList();
        }

       
    }
}
