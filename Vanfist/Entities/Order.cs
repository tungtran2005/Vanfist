using Vanfist.Constants;
namespace Vanfist.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public OrderItem Item { get; set; }
        public string Status { get; set; } = Constants.Invoice.Status.Pending;
        
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
