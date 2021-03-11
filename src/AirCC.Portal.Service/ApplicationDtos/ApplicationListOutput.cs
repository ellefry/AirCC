using System;
using System.Collections.Generic;
using System.Text;
using BCI.Extensions.DDD.ApplicationService;

namespace AirCC.Portal.AppService.ApplicationDtos
{
    public class ApplicationListOutput
    {
        public ApplicationListOutput()
        { }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ClientSecret { get; set; }
    }
}
