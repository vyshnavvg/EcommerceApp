using Core.Entities.OrderAggregate;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public string CartId { get; set; } = string.Empty;
        [Required]
        public int DeliverymethodId { get; set; }
        [Required]
        public ShippingAddress ShippingAddress { get; set; } = null!;
        [Required]
        public PaymentSummary PaymentSummary { get; set; } = null!;
    }
}
