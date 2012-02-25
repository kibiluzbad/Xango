using System;

namespace Xango.Data
{
    public interface IStampedEntity
    {
        string CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        string ChangedBy { get; set; }
        DateTime? ChangedAt { get; set; }
    }
}