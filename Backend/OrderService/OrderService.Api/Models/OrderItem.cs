namespace OrderService.Api.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }  // ✅ GUID
        public Guid OrderId { get; set; }  // ✅ GUID
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtTime { get; set; }

        // relation inverse :
        public Order Order { get; set; } = null!;
    }
}

