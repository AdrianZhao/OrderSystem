namespace OrderSystem.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public ICollection<InCartProduct> Products { get; set; }
    }
}
