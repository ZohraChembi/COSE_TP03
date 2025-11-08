using Microsoft.EntityFrameworkCore;
using OrderService.Api.Models;
using OrderService.Data;

namespace OrderService.Api.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Créer un panier
        public async Task<Cart> CreateCartAsync(Guid userId)
        {
            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Items = new List<CartItem>()
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        // Ajouter un item au panier
        public async Task<CartItem> AddItemToCartAsync(Guid cartId, Guid productId, int quantity)
        {
            var item = new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity
            };

            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        // Obtenir le panier avec ses items
        public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // Supprimer un item du panier
        public async Task<bool> RemoveItemAsync(Guid cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item == null) return false;

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        // Supprimer le panier
        public async Task<bool> DeleteCartAsync(Guid cartId)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null) return false;

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
