using DNT.Deskly.Domain;

namespace DNT.Deskly.Configuration
{
    public class KeyValue : IHasRowVersion, IHasRowIntegrity, ICreationTracking, IModificationTracking
    {
        public virtual string Key { get; set; }
        public virtual string Value { get; set; }
        public byte[] Version { get; set; }
    }
}