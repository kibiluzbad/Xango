using System;
using System.Data.SqlTypes;

namespace Blog.Domain
{
    public class Period : INullable
    {
        public Period(DateTime start, DateTime end)
        {
            Validate(start, end);
            Start = start;
            End = end;
        }

        public virtual DateTime? Start { get; protected set; }
        public virtual DateTime? End { get; protected set; }

        #region INullable Members

        public bool IsNull
        {
            get { return !Start.HasValue || !End.HasValue; }
        }

        #endregion

        protected virtual void Validate(DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue &&
                start.Value > end.Value)
                throw new InvalidOperationException("Start must be greater than or eaquals End.");
        }
    }
}