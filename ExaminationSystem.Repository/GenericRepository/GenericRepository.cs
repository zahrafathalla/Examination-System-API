﻿using ExaminationSystem.Core.Contracts;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.Specification;
using ExaminationSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Repository.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbContext.Set<T>().ToListAsync();
        

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression)
            => await _dbContext.Set<T>().Where(expression).ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(T entity)
            => await _dbContext.AddAsync(entity);

        public void Update(T entity)
            => _dbContext.Update(entity);

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _dbContext.Update(entity);
        }

        public async Task<IEnumerable<T>> GetAllWithSpecificationAsync(BaseSpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T?> GetByIdWithSpecificationAsync(BaseSpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();

        }

        private IQueryable<T> ApplySpecification( BaseSpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }
    }
}
