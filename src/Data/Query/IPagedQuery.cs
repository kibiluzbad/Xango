namespace Xango.Data.Query
{
    public interface IPagedQuery<T> : IQuery<PagedResult<T>>
    {
        int PageNumber { get; set; }
        int ItemsPerPage { get; set; }
    }
}