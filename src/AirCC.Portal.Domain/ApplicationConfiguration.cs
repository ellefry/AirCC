using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCI.Extensions.Entity;

namespace AirCC.Portal.Domain
{
    public class ApplicationConfiguration : FullAuditEntity<string>
    {
        public string CfgKey { get; set; }
        public string CfgValue { get; set; }
        public CfgStatus Status { get; set; } = CfgStatus.Offline;

        public IEnumerable<ApplicationConfigurationHistory> ConfigurationHistories => _configurationHistories.ToList();

        private ICollection<ApplicationConfigurationHistory> _configurationHistories;

        public void Renew(string key, string value)
        {
            this.CfgKey = key;
            this.CfgValue = value;
            Offline();
        }

        public void Online()
        {
            this.Status = CfgStatus.Online;
        }
        public void Offline()
        {
            this.Status = CfgStatus.Offline;
        }
    }

    public enum CfgStatus
    {
        Offline,
        Online
    }
}
