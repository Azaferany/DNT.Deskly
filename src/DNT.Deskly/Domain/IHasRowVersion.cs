namespace DNT.Deskly.Domain
{
    public interface IHasRowVersion
    {
        byte[] Version { get; set; }
    }
}