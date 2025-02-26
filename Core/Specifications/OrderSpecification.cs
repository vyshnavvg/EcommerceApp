using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(string email) : base(x => x.BuyerEmail == email)
        {
            AddInclude(x => x.orderItems);
            AddInclude(x => x.DeliveryMethod);
            AddOrderByDescending(x => x.OrderDate);  
        }

        public OrderSpecification(string email, int id) : base(x => x.BuyerEmail == email && x.Id == id)
        {
            AddInculde("orderItems");
            AddInculde("DeliveryMethod");
        }
    }
}
