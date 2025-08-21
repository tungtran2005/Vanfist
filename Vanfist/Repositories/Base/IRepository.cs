namespace Vanfist.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> FindById(int id);
    Task<IEnumerable<T>> FindAll();
    Task<T> Save(T entity);
    Task Update(T entity);
    Task Delete(T entity);
    Task SaveChanges();
}