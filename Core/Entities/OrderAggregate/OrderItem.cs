namespace Core.Entities.OrderAggregate
{
    internal class OrderItem : BaseEntity
    {
        public ProductItemOrdered ItemOrdered { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
