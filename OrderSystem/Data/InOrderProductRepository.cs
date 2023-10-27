using Microsoft.EntityFrameworkCore;
using OrderSystem.Models;

namespace OrderSystem.Data
{
    public class InOrderProductRepository : IRepository<InOrderProduct>
    {
        private OrderSystemContext _context;
        public InOrderProductRepository(OrderSystemContext context)
        {
            _context = context;
        }
        public InOrderProduct Create(InOrderProduct entity)
        {
            _context.InOrderProducts.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(InOrderProduct entity)
        {
            _context.InOrderProducts.Remove(entity);
            _context.SaveChanges();
        }
        public InOrderProduct Get(Guid id)
        {
            InOrderProduct inOrderProduct = _context.InOrderProducts.Find(id);
            return inOrderProduct;
        }
        public ICollection<InOrderProduct> GetAll()
        {
            ICollection<InOrderProduct> inOrderProducts = _context.InOrderProducts.Include(p => p.Product).ToHashSet();
            return inOrderProducts;
        }
        public InOrderProduct Update(InOrderProduct entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
