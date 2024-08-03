using ExaminationSystem.Core.Entities;
using System.Linq.Expressions;

namespace ExaminationSystem.Core.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> expression);
        Task<T?> GetByIdAsync (int id);
        Task AddAsync (T entity);
        void Delete(T entity);
        void Update (T entity);

    }
}
