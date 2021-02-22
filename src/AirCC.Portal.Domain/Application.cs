using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BCI.Extensions.Entity;

namespace AirCC.Portal.Domain
{
    public class Application : FullAuditEntity<string>
    {
        public string Name { get; set; }
        public string ClientSecret { get; set; }
        public bool Active { get; set; } = true;

        public IEnumerable<ApplicationConfiguration> Configurations => _configurations.ToList();

        private ICollection<ApplicationConfiguration> _configurations;

        public void AddConfiguration([NotNull] ApplicationConfiguration cfg)
        {
            if(_configurations == null)
                throw  new ApplicationException("You must first retrieve this application's configuration list.");
            var foundConf = _configurations
                .FirstOrDefault(c => c.CfgKey == cfg.CfgKey && c.CfgValue == cfg.CfgValue);
            if (foundConf != null)
                throw new ApplicationException("Duplicate configuration.");
            _configurations.Add(cfg);
        }

    }
}
