﻿using System;
using System.Collections.Generic;
using System.Text;
using BCI.Extensions.Entity;

namespace AirCC.Portal.Domain
{
    public class ApplicationConfiguration : FullAuditEntity<string>
    {
        public string ApplicationId { get; set; }
        public string CfgKey { get; set; }
        public string CfgValue { get; set; }
        public CfgStatus Status { get; set; }
    }

    public enum CfgStatus
    {
        Offline,
        Online
    }
}
