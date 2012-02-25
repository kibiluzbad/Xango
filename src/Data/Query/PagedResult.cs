using System.Collections.Generic;

namespace Xango.Data.Query
{
    public class PagedResult<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> PageOfResults { get; set; }
    }
}