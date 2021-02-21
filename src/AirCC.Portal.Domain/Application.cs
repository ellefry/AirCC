using System;
using System.Collections.Generic;
using System.Linq;
using BCI.Extensions.Entity;

namespace AirCC.Portal.Domain
{
    public class Application : FullAuditEntity<string>
    {
        public string Name { get; set; }
        public string ClientSecret { get; set; }
        public bool Active { get; set; }

        public IEnumerable<ApplicationConfiguration> Configurations => _configurations.ToList();

        private ICollection<ApplicationConfiguration> _configurations; 


    }
}
