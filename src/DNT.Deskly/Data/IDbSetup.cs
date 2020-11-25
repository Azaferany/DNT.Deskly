using DNT.Deskly.Dependency;

namespace DNT.Deskly.Data
{
    public interface IDbSetup : ITransientDependency
    {
        void Seed();
    }
}