using System;
using System.Collections.Generic;
using System.Text;
using BCI.Extensions.Entity;

namespace AirCC.Portal.Domain
{
    public class ApplicationConfigurationHistory : FullAuditEntity<string>
    {
        public string CfgKey { get; set; }
        public string CfgValue { get; set; }
    }
}
