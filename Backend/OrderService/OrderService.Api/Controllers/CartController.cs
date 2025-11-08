using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Models;
using OrderService.Api.Services;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        // GET api/cart/{userId} → obtenir le panier d'un utilisateur
        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(Guid userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        // POST api/cart → créer un nouveau panier
        [HttpPost]
        public async Task<ActionResult<Cart>> CreateCart([FromBody] Guid userId)
        {
            var cart = await _cartService.CreateCartAsync(userId);
            return Ok(cart);
        }

        // POST api/cart/{cartId}/items → ajouter un item
        [HttpPost("{cartId}/items")]
        public async Task<ActionResult> AddItem(Guid cartId, [FromBody] CartItem item)
        {
            var addedItem = await _cartService.AddItemToCartAsync(cartId, item.ProductId, item.Quantity);
            return Ok(addedItem);
        }

        // DELETE api/cart/items/{cartItemId} → supprimer un item
        [HttpDelete("items/{cartItemId}")]
        public async Task<ActionResult> RemoveItem(Guid cartItemId)
        {
            var success = await _cartService.RemoveItemAsync(cartItemId);
            if (!success) return NotFound();
            return NoContent();
        }

        // DELETE api/cart/{cartId} → supprimer le panier
        [HttpDelete("{cartId}")]
        public async Task<ActionResult> DeleteCart(Guid cartId)
        {
            var success = await _cartService.DeleteCartAsync(cartId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
