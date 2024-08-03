using ExaminationSystem.Core;
using ExaminationSystem.Core.Contracts;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Repository.Data;
using ExaminationSystem.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Repository
{
    public class UnitOfWork : IunitOfWork
    {
        private Hashtable _repository; // (key) name or type of repo /(value) generic repo
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext) 
        {
            _repository = new Hashtable();
            _dbContext = dbContext;
        }
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var key = typeof(T);                             //type of repository

            if(!_repository.ContainsKey(key))
            {
                var value = new GenericRepository<T>(_dbContext);   //repository

                _repository.Add(key,value);
            }

            return _repository[key] as IGenericRepository<T>; 
        }
        public async Task<int> CompleteAsync()
        {
            //HandleSoftDelete();
           return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
            => _dbContext.Dispose();

        //private void HandleSoftDelete()
        //{
        //    var deletedEntities = _dbContext.ChangeTracker.Entries<BaseEntity>()
        //        .Where(e => e.State == EntityState.Deleted); //entities marked as deleted

        //    foreach (var entry in deletedEntities)
        //    {
        //        entry.State = EntityState.Modified;
        //        entry.Entity.IsDeleted = true;
        //    }
        //}

    }
}
