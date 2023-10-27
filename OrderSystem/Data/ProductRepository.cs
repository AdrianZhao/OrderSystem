using OrderSystem.Models;

namespace OrderSystem.Data
{
    public class ProductRepository : IRepository<Product>
    {
        private OrderSystemContext _context;
        public ProductRepository(OrderSystemContext context)
        {
            _context = context;
        }
        public Product Create(Product entity)
        {
            _context.Products.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(Product entity)
        {
            _context.Products.Remove(entity);
            _context.SaveChanges();
        }
        public Product Get(Guid id)
        {
            Product product = _context.Products.Find(id);
            return product;
        }
        public ICollection<Product> GetAll()
        {
            return _context.Products.ToHashSet();
        }
        public Product Update(Product entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
