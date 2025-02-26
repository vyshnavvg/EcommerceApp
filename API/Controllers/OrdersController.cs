using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController: BaseApiController
    {
        private readonly ICartService cartService;
        private readonly IUnitOfWork unitOfWork;
        
        public OrdersController(ICartService cartService, IUnitOfWork unitOfWork)
        {
            this.cartService = cartService;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Createorder(CreateOrderDto orderDto)
        {
            var email = User.GetEmail();

            var cart = await cartService.GetCartAsync(orderDto.CartId);

            if (cart == null) return BadRequest("Cart not found");

            if (cart.PaymentIntentId == null) return BadRequest("No payment intent for this order");

            var items = new List<OrderItem>();

            foreach (var item in cart.Items)
            {
                var productItem = await unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);

                if (productItem == null) return BadRequest("Problem with the order");

                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    PictureUrl = item.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
            }

            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliverymethodId);

            if (deliveryMethod == null) return BadRequest("No delivey method selected");

            var order = new Order
            {
                orderItems = items,
                DeliveryMethod = deliveryMethod,
                ShippingAddress = orderDto.ShippingAddress,
                SubTotal = items.Sum(x => x.Price * x.Quantity),
                PaymentSummary = orderDto.PaymentSummary,
                PaymentIntentId = cart.PaymentIntentId,
                BuyerEmail = email
            };

            unitOfWork.Repository<Order>().Add(order);

            if(await unitOfWork.Complete())
            {
                return order;
            }

            return BadRequest("Problem creating order");

        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {
            var spec = new OrderSpecification(User.GetEmail());

            var orders = await unitOfWork.Repository<Order>().ListAsync(spec);

            var ordersToReturn = orders.Select( o => o.ToDto()).ToList();

            return Ok(ordersToReturn);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var spec = new OrderSpecification(User.GetEmail(), id);

            var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null) return NotFound();

            return Ok(order.ToDto());
        }
    }
}
