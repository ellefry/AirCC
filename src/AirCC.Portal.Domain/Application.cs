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
            ApplicationConfiguration foundConf = GetConfiguration(configurationId);
            foundConf.UpdateValue(newValue);
        }

        public void Online(string configurationId)
        {
            ApplicationConfiguration foundConf = GetConfiguration(configurationId);
            foundConf.Online();
        }

        public void Offline(string configurationId)
        {
            ApplicationConfiguration foundConf = GetConfiguration(configurationId);
            foundConf.Offline();
        }

        public IEnumerable<ApplicationConfiguration> GetConfigurations()
        {
            return Configurations;
        }

        private ApplicationConfiguration GetConfiguration(string configurationId)
        {
            var foundConf = Configurations.FirstOrDefault(c => c.Id == configurationId);
            if (foundConf == null)
                throw new ApplicationException($"No configuration was found, [{configurationId}].");
            return foundConf;
        }

        public void DeleteConfiguration(string configurationId)
        {
            ApplicationConfiguration foundConf = GetConfiguration(configurationId);
            Configurations.Remove(foundConf);
        }
    }
}
