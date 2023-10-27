using Microsoft.EntityFrameworkCore;
using OrderSystem.Models;

namespace OrderSystem.Data
{
    public class CartRepository : IRepository<Cart>
    {
        private OrderSystemContext _context;
        public CartRepository(OrderSystemContext context)
        {
            _context = context;
        }
        public Cart Create(Cart entity)
        {
            _context.Carts.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(Cart entity)
        {
            _context.Carts.Remove(entity);
            _context.SaveChanges();
        }
        public Cart Get(Guid id)
        {
            Cart cart = _context.Carts.Find(id);
            return cart;
        }
        public ICollection<Cart> GetAll()
        {
            ICollection<Cart> carts = _context.Carts.Include(c => c.Products).ThenInclude(p => p.Product).ToHashSet();
            return carts;
        }
        public Cart Update(Cart entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
