using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Portal.AppService.ApplicationDtos
{
    public class CreateConfigurationInput
    {
        public string Id { get; set; }
        public string ApplicationId { get; set; }
        public string CfgKey { get; set; }
        public string CfgValue { get; set; }
    }
}
