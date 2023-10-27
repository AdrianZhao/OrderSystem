using Microsoft.EntityFrameworkCore;
using OrderSystem.Models;

namespace OrderSystem.Data
{
    public class OrderRepository : IRepository<Order>
    {
        private OrderSystemContext _context;
        public OrderRepository(OrderSystemContext context)
        {
            _context = context;
        }
        public Order Create(Order entity)
        {
            _context.Orders.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(Order entity)
        {
            _context.Orders.Remove(entity);
            _context.SaveChanges();
        }
        public Order Get(Guid id)
        {
            Order order = _context.Orders.Find(id);
            return order;
        }
        public ICollection<Order> GetAll()
        {
            ICollection<Order> orders = _context.Orders.Include(o => o.Products).ThenInclude(p => p.Product).ToHashSet();
            return orders;
        }
        public Order Update(Order entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
