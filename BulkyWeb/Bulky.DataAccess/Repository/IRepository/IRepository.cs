using System.Linq.Expressions;

namespace Bulky.DataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
    // T - Category
    IEnumerable<T> GetAll();
    T GetFirstOfDefault(Expression<Func<T, bool>> filter);
    void Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}
