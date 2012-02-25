using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xango.Data.NHibernate.Queries
{
    public interface IGetEntityById<TEntity>
       : Query.IQuery<TEntity> where TEntity : Entity
    {
        long Id { get; set; }
    }
}
