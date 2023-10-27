namespace OrderSystem.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public ICollection<InOrderProduct> Products { get; set; }
        public string DestinationCountry { get; set; }
        public string Address { get; set; }
        public string MailingCode { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
