using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Portal.Domain
{
    public class ApplicationConfiguration
    {
        public string ApplicationId { get; set; }
        public string CfgKey { get; set; }
        public string CfgValue { get; set; }
        public int? Version { get; set; }
    }
}
