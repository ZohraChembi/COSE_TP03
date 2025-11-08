using Microsoft.EntityFrameworkCore;
using OrderService.Api.Models;
using OrderService.Data;

namespace OrderService.Api.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Créer une commande à partir d’un panier
        public async Task<Order> CreateOrderFromCartAsync(Cart cart)
        {
            if (cart.Items.Count == 0)
                throw new InvalidOperationException("Le panier est vide");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = cart.UserId,
                Status = "Pending",
                Items = cart.Items.Select(ci => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    PriceAtTime = 0 // à remplir selon ton catalogue produit
                }).ToList()
            };

            // Calculer le total (supposons PriceAtTime est déjà renseigné)
            order.TotalAmount = order.Items.Sum(i => i.PriceAtTime * i.Quantity);

            _context.Orders.Add(order);

            // Supprimer le panier après la commande
            _context.Carts.Remove(cart);

            await _context.SaveChangesAsync();
            return order;
        }

        // Obtenir toutes les commandes d’un utilisateur
        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        // Mettre à jour le statut d’une commande
        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
