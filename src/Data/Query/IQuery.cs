namespace Xango.Data.Query
{
    public interface IQuery
    {
    }

    public interface IQuery<TResult> : IQuery
    {
        TResult Execute();
    }
}