using System.ComponentModel.DataAnnotations;

namespace Vanfist.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public int ModelId { get; set; }
        public Model Model { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải >= 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải > 0")]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
