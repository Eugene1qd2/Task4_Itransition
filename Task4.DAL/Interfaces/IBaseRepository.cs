namespace Task4.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        public Task<bool> Create(T obj);
        public Task<T> GetById(int id);
        public Task<List<T>> GetAll();
        public Task<bool> Delete(T obj);
        public Task<bool> Update(T obj);
    }
}
