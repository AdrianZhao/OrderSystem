namespace OrderSystem.Data
{
    public interface IRepository<T> where T : class
    {
        public T Get(Guid id);
        public ICollection<T> GetAll();
        public T Create(T entity);
        public T Update(T entity);
        public void Delete(T entity);
    }   
}
