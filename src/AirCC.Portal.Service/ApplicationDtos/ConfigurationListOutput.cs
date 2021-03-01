using AirCC.Portal.Domain;
using BCI.Extensions.DDD.ApplicationService;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Portal.AppService.ApplicationDtos
{
    public class ConfigurationListOutput
    {
        public string Id { get; set; }
        public string CfgKey { get; set; }
        public string CfgValue { get; set; }
        public CfgStatus Status { get; set; }
        public string ApplicationId { get; set; }
    }

    public class ConfigurationListInput : PagedInputDto
    {
        public string CfgKey { get; set; }
        public CfgStatus Status { get; set; }

    }
}
