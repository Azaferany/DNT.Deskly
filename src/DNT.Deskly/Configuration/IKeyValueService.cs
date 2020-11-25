using System.Threading.Tasks;
using DNT.Deskly.Application;
using DNT.Deskly.Functional;

namespace DNT.Deskly.Configuration
{
    public interface IKeyValueService : IApplicationService
    {
        Task SetValueAsync(string key, string value);
        Task<Maybe<string>> LoadValueAsync(string key);
    }
}
