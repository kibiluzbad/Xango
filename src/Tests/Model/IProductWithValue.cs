using Xango.Data.Query;

namespace Xango.Tests.Model
{
    public interface IProductWithValue : IQuery<Product>
    {
        decimal Value { get; set; }
    }
}