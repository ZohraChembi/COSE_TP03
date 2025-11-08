namespace OrderService.Api.Models
{
    public class Order
    {
        public Guid Id { get; set; }  // ✅ GUID
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

   

}

