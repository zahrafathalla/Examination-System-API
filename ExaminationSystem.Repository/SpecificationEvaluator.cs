using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, BaseSpecification<T> spec)
        {
            var query = inputQuery; //_dbcontext.set<T>()

            if (spec.Criteria != null)
                query = query.Where(spec.Criteria); 

            query = spec.Include.Aggregate(query, (currentQuery, icludeExpression) => currentQuery.Include(icludeExpression));

            return query;
        }
    }
}
