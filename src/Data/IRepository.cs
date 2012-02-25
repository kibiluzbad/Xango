using System.Collections.Generic;
using LinqSpecs;
using Xango.Data.Query;

namespace Xango.Data
{
    public interface IRepository<TEntity> : IEnumerable<TEntity>, IQueryFactory
        where TEntity : Entity
    {
        int Count { get; }
        void Add(TEntity item);
        bool Contains(TEntity item);
        bool Remove(TEntity item);
        IEnumerable<TEntity> FindAll(Specification<TEntity> specification);
        TEntity FindOne(Specification<TEntity> specification);
    }
}