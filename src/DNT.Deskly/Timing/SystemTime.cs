using System;

namespace DNT.Deskly.Timing
{
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.UtcNow;

        public static Func<DateTime, DateTime> Normalize = dateTime =>
            DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}