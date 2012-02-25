using System;
using System.Linq.Expressions;
using LinqSpecs;

namespace Xango.Data.NHibernate.Specifications
{
    public class GenericSpecification<T>
        : Specification<T>
        where T : Entity
    {
        private readonly Expression<Func<T, bool>> _expression;

        public GenericSpecification(Expression<Func<T, bool>> expression)
        {
            _expression = expression;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            return _expression;
        }
    }
}