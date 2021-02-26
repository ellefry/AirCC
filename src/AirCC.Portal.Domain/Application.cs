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

        public virtual ICollection<ApplicationConfiguration> Configurations { get; private set; }
        
        public void AddConfiguration([NotNull] ApplicationConfiguration cfg)
        {
            if (Configurations == null)
                throw new ApplicationException("You must first retrieve this application's configuration list.");
            var existed = Configurations
                .Any(c => c.CfgKey == cfg.CfgKey && c.CfgValue == cfg.CfgValue);
            if (existed)
                throw new ApplicationException("Duplicate configuration.");
            Configurations.Add(cfg);
        }

        public void UpdateConfigurationValue([NotNull] string configurationId, string newValue)
        {
            //f (cfg.CfgValue == newValue) return;
            var foundConf = Configurations.FirstOrDefault(c => c.Id == configurationId);
            if (foundConf == null)
                throw new ApplicationException($"No configuration was found, [{configurationId}].");

        }

    }
}
