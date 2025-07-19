namespace Shop.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public decimal ShippingCode { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PaymentMethod { get; set; }
        public int Status { get; set; }
    }
}
