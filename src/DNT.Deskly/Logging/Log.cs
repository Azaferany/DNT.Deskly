﻿using System;
using DNT.Deskly.Domain;

// ReSharper disable InconsistentNaming

namespace DNT.Deskly.Logging
{
    /// <summary>
    /// Represents a log in the logging database.
    /// </summary>
    public class Log : IEntity<int>
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public DateTime CreationTime { get; set; }
        public string LoggerName { get; set; }
        public string UserBrowserName { get; set; }
        public string UserIP { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserDisplayName { get; set; }
        public string ImpersonatorUserId { get; set; }
        public int EventId { get; set; }
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public string ImpersonatorTenantId { get; set; }
    }
}