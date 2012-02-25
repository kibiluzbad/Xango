namespace Xango.Data.Query
{
    public interface IQueryFactory
    {
        TQuery CreateQuery<TQuery>() where TQuery : IQuery;
    }
}