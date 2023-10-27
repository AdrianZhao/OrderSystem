using Microsoft.EntityFrameworkCore;
using OrderSystem.Models;
using System.Collections.Generic;

namespace OrderSystem.Data
{
    public class InCartProductRepository : IRepository<InCartProduct>
    {
        private OrderSystemContext _context;
        public InCartProductRepository(OrderSystemContext context)
        {
            _context = context;
        }
        public InCartProduct Create(InCartProduct entity)
        {
            _context.InCartProducts.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(InCartProduct entity)
        {
            _context.InCartProducts.Remove(entity);
            _context.SaveChanges();
        }
        public InCartProduct Get(Guid id)
        {
            InCartProduct inCartProduct = _context.InCartProducts.Find(id);
            return inCartProduct;
        }
        public ICollection<InCartProduct> GetAll()
        {
            ICollection <InCartProduct> inCartProducts = _context.InCartProducts.Include(p => p.Product).ToHashSet();
            return inCartProducts;
        }
        public InCartProduct Update(InCartProduct entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
