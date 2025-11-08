using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Models;
using OrderService.Api.Services;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService.Api.Services.OrderService _orderService;
        private readonly CartService _cartService;

        public OrderController(OrderService.Api.Services.OrderService orderService, CartService cartService)
        {
            _orderService = orderService;
            _cartService = cartService;
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<Order>> CreateOrder(Guid userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null || cart.Items.Count == 0)
                return BadRequest("Le panier est vide");

            var order = await _orderService.CreateOrderFromCartAsync(cart);
            return Ok(order);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<Order>>> GetOrders(Guid userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpPatch("{orderId}/status")]
        public async Task<ActionResult> UpdateStatus(Guid orderId, [FromBody] string status)
        {
            var success = await _orderService.UpdateOrderStatusAsync(orderId, status);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
