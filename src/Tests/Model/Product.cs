using Xango.Data;

namespace Xango.Tests.Model
{
    public class Product : Entity
    {
        public virtual string Name { get; set; }
        public virtual decimal Value { get; set; }
    }
}