using DNT.Deskly.Domain;
using System;

namespace DNT.Deskly.Caching
{
    public class Cache : IEntity<string>
    {
        public byte[] Value { get; set; }
        public DateTimeOffset ExpiresAtTime { get; set; }
        public long? SlidingExpirationInSeconds { get; set; }
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        public string Id { get; set; }
    }
}