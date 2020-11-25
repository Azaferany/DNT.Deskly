using System;
using System.Collections.Generic;
using System.Text;

namespace DNT.Deskly.Web.Middlewares
{
    public class ContentSecurityPolicyConfig
    {
        /// <summary>
        /// CSP options. Each options should be specified in one line.
        /// </summary>
        public string[] Options { get; set; } = { "default-src 'self' blob:",
      "style-src 'self' 'unsafe-inline'",
      "script-src 'self' 'unsafe-inline' 'unsafe-eval' ",
      "font-src 'self'",
      "img-src 'self' data: blob:",
      "connect-src 'self'",
      "media-src 'self'",
      "object-src 'self' blob:"};

        /// <summary>
        /// is Content Security Policy LogEndpoint available 
        /// default value is false
        /// </summary>
        public bool IsLogEndpointAvailable { get; set; } = false;
        /// <summary>
        /// ContentSecurityPolicyLogEndpoint
        /// </summary>
        public string LogEndpoint { get; set; }

    }
}
