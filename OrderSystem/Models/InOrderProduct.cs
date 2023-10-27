namespace OrderSystem.Models
{
    public class InOrderProduct
    {
        public Guid Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
