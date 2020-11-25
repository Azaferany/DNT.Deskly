using System;
using DNT.Deskly.Dependency;

namespace DNT.Deskly.Timing
{
    /// <summary>
    /// Defines the required contract for implementing a clock.
    /// </summary>
    public interface IClock : ITransientDependency
    {
        DateTime Now { get; }
        DateTime Normalize(DateTime dateTime);
    }

    internal sealed class Clock : IClock
    {
        public DateTime Now => SystemTime.Now();

        public DateTime Normalize(DateTime dateTime)
        {
            return SystemTime.Normalize(dateTime);
        }
    }
}