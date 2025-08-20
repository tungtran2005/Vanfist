namespace Vanfist.Constants;

public class Invoice
{
    public static class Status
    {
        public const string Pending = "Pending"; // Chờ xử lý
        public const string Paid = "Paid"; // Đã thanh toán
        public const string Cancelled = "Cancelled"; // Đã hủy
    }
    
    public static class Type
    {
        public const string Service = "Advisory"; // Tư vấn
        public const string Repair = "Deposit"; // Đặt cọc
    }
}