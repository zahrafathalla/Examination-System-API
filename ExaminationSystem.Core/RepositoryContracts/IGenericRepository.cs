using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.Specification;
using System.Linq.Expressions;

namespace ExaminationSystem.Core.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllWithSpecificationAsync(BaseSpecification<T> spec);
        Task<T?> GetByIdWithSpecificationAsync(BaseSpecification<T> spec);
        Task AddAsync (T entity);
        void Delete(T entity);
        void Update (T entity);

    }
}
