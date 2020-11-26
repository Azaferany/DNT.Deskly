using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DNT.Deskly.Cryptography;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DNT.Deskly.EFCore.Cryptography
{
    /// <summary>
    /// An <see cref="IXmlRepository"/> backed by an EntityFrameworkCore datastore.
    /// </summary>
    public class XmlRepository<TContext> : IXmlRepository
        where TContext : DbContext
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;

        public XmlRepository(IServiceProvider provider, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<XmlRepository<TContext>>();
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <inheritdoc />
        public virtual IReadOnlyCollection<XElement> GetAllElements()
        {
            using var scope = _provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            var logger = _logger;
            return context.Set<ProtectionKey>().AsNoTracking().Select(key => TryParseKeyXml(key.XmlValue, logger))
                .ToList()
                .AsReadOnly();
        }

        /// <inheritdoc />
        public void StoreElement(XElement element, string friendlyName)
        {
            using var scope = _provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            var key = new ProtectionKey
            {
                FriendlyName = friendlyName,
                XmlValue = element.ToString(SaveOptions.DisableFormatting)
            };

            context.Set<ProtectionKey>().Add(key);
            _logger.LogSavingKeyToDbContext(friendlyName, typeof(TContext).Name);
            context.SaveChanges();
        }

        private static XElement TryParseKeyXml(string xml, ILogger logger)
        {
            try
            {
                return XElement.Parse(xml);
            }
            catch (Exception e)
            {
                logger?.LogExceptionWhileParsingKeyXml(xml, e);
                return null;
            }
        }
    }
}