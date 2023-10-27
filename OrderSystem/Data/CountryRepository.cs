using OrderSystem.Models;

namespace OrderSystem.Data
{
    public class CountryRepository : IRepository<Country>
    {
        private OrderSystemContext _context;
        public CountryRepository(OrderSystemContext context)
        {
            _context = context;
        }
        public Country Create(Country entity)
        {
            _context.Countries.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(Country entity)
        {
            _context.Countries.Remove(entity);
            _context.SaveChanges();
        }
        public Country Get(Guid id)
        {
            Country countries = _context.Countries.Find(id);
            return countries;
        }
        public ICollection<Country> GetAll()
        {
            return _context.Countries.ToHashSet();
        }
        public Country Update(Country entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
