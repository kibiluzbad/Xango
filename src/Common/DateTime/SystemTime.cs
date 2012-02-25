using System;

namespace Xango.Common.DateTime
{
    public static class SystemTime
    {
        public static Func<System.DateTime> Now = () => System.DateTime.Now;
    }
}
