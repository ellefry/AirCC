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
            for (int count = 0; count < 10000; count++)
            {
                var c = new ApplicationConfiguration {CfgKey = count + "_key", CfgValue = count + "_value"};
                Configurations.Add(c);
            }

            //if (Configurations == null)
            //    throw new ApplicationException("You must first retrieve this application's configuration list.");
            //var foundConf = Configurations
            //    .FirstOrDefault(c => c.CfgKey == cfg.CfgKey && c.CfgValue == cfg.CfgValue);
            //if (foundConf != null)
            //    throw new ApplicationException("Duplicate configuration.");
            //Configurations.Add(cfg);
        }

    }
}
