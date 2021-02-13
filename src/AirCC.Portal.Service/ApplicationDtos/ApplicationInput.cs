using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Portal.AppService.ApplicationDtos
{
    public class ApplicationInput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ClientSecret { get; set; }
        public bool Active { get; set; }
    }
}
