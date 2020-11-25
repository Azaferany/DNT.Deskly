using System;
using System.Threading.Tasks;
using DNT.Deskly.Application;
using DNT.Deskly.Configuration;
using DNT.Deskly.EFCore.Context;
using DNT.Deskly.Functional;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DNT.Deskly.EFCore.Configuration
{
    internal sealed class KeyValueService : ApplicationService, IKeyValueService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly DbSet<KeyValue> _values;

        public KeyValueService(IUnitOfWork uow, IConfiguration configuration)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _values = _uow.Set<KeyValue>();
        }

        public async Task SetValueAsync(string key, string value)
        {
            var record = await _values.FirstOrDefaultAsync(v => v.Key == key);
            if (record == null)
            {
                _values.Add(new KeyValue
                {
                    Key = key,
                    Value = value
                });
            }
            else
            {
                record.Value = value;
            }

            await _uow.SaveChangesAsync();

            ReloadConfiguration();
        }

        public async Task<Maybe<string>> LoadValueAsync(string key)
        {
            var keyValue = await _values.FirstOrDefaultAsync(v => v.Key == key);
            return keyValue == null ? Maybe<string>.None : keyValue.Value;
        }

        private void ReloadConfiguration()
        {
            ((IConfigurationRoot) _configuration).Reload();
        }
    }
}