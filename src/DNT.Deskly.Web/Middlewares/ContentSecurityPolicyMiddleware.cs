using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DNT.Deskly.Web.Middlewares
{
    public sealed class ContentSecurityPolicyMiddleware
    {
        private const string XFrameOptions = "X-Frame-Options";
        private const string XXssProtection = "X-Xss-Protection";
        private const string XContentTypeOptions = "X-Content-Type-Options";
        private const string ContentSecurityPolicy = "Content-Security-Policy";

        private readonly RequestDelegate _next;
        private readonly string _policyValue;
        private readonly ContentSecurityPolicyConfig _configuration;

        public ContentSecurityPolicyMiddleware(RequestDelegate next, IConfiguration configuration, IOptions<ContentSecurityPolicyConfig> options)
        {
            _next = next;
            _configuration = options.Value ?? new ContentSecurityPolicyConfig();
            _policyValue = CreatePolicyValue();
        }

        private string CreatePolicyValue()
        {
            string[] csp;
            if (_configuration.IsLogEndpointAvailable)
            {
                if (string.IsNullOrWhiteSpace(_configuration.LogEndpoint))
                {
                    throw new NullReferenceException(
                        "Please set the `ContentSecurityPolicyLogEndpoint` value in `appsettings.json` file.");
                }

                csp = new string[]
                {
                "default-src 'self' blob:",
                "style-src 'self' 'unsafe-inline'",
                "script-src 'self' 'unsafe-inline' 'unsafe-eval' ",
                "font-src 'self'",
                "img-src 'self' data: blob:",
                "connect-src 'self'",
                "media-src 'self'",
                "object-src 'self' blob:",
                $"report-uri {_configuration.LogEndpoint}"
            };
            }
            else
            {
                csp = new string[]
                {
                "default-src 'self' blob:",
                "style-src 'self' 'unsafe-inline'",
                "script-src 'self' 'unsafe-inline' 'unsafe-eval' ",
                "font-src 'self'",
                "img-src 'self' data: blob:",
                "connect-src 'self'",
                "media-src 'self'",
                "object-src 'self' blob:",
                };
            }
            return string.Join("; ", csp);
        }

        public Task Invoke(HttpContext context)
        {
            if (!context.Response.Headers.ContainsKey(XFrameOptions))
            {
                context.Response.Headers.Add(XFrameOptions, "SAMEORIGIN");
            }

            if (!context.Response.Headers.ContainsKey(XXssProtection))
            {
                context.Response.Headers.Add(XXssProtection, "1; mode=block");
            }

            if (!context.Response.Headers.ContainsKey(XContentTypeOptions))
            {
                context.Response.Headers.Add(XContentTypeOptions,
                    "nosniff"); // Refused to execute script from '<URL>' because its MIME type ('') is not executable, and strict MIME type checking is enabled.
            }

            if (!context.Response.Headers.ContainsKey(ContentSecurityPolicy))
            {
                context.Response.Headers.Add(ContentSecurityPolicy, _policyValue);
            }

            return _next(context);
        }
    }
}