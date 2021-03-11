using BCI.Extensions.DDD.ApplicationService;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Portal.AppService.ApplicationDtos
{
    public class ApplicationListInput : PagedInputDto
    {
        public string Name { get; set; }
    }
}
