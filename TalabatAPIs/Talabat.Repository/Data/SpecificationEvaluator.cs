using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity 
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if(spec.Criteria != null)
                query = query.Where(spec.Criteria);

            if(spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            if (spec.OrederBy != null)
                query = query.OrderBy(spec.OrederBy);

            if(spec.OrederByDescending != null)
                query = query.OrderByDescending(spec.OrederByDescending);

            query = spec.Includes.Aggregate(query , (currentQuery , includeExpression) => currentQuery.Include(includeExpression));
            
            return query;
        }
    }
}
