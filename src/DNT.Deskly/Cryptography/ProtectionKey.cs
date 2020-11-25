using DNT.Deskly.Domain;

namespace DNTFrameworkCore.Cryptography
{
    public class ProtectionKey : IEntity<long>
    {
        public string FriendlyName { get; set; }
        public string XmlValue { get; set; }
        public long Id { get; set; }
    }
}