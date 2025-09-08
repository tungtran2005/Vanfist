using System.ComponentModel.DataAnnotations;
namespace Vanfist.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }
        public Account Account { get; set; }

        [Required]
        public OrderItem Item { get; set; }

        [Required, StringLength(50)]
        public string Status { get; set; } = Constants.Invoice.Status.Pending;
    }
}
