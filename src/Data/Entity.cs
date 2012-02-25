using System;

namespace Xango.Data
{
    public abstract class Entity<TId>
    {
        public virtual TId Id { get; protected set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<TId>);
        }

        private static bool IsTransient(Entity<TId> obj)
        {
            return obj != null &&
                   Equals(obj.Id, default(TId));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(Entity<TId> other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                Type otherType = other.GetUnproxiedType();
                Type thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                       otherType.IsAssignableFrom(thisType);
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(TId)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }
    }

    public abstract class Entity : Entity<long>
    {
    }

    public abstract class StampedEntity : Entity, IStampedEntity
    {
        protected StampedEntity()
        {
            CreatedAt = DateTime.Now;
        }

        #region IStampedEntity Members

        public virtual string CreatedBy { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual string ChangedBy { get; set; }

        public virtual DateTime? ChangedAt { get; set; }

        #endregion
    }
}