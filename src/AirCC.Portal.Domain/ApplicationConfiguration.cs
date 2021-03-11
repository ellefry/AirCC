using BCI.Extensions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirCC.Portal.Domain
{
    public class ApplicationConfiguration : FullAuditEntity<string>
    {
        //public string ApplicationId { get; set; }
        public string CfgKey { get; set; }
        public string CfgValue { get; set; }
        public CfgStatus Status { get; set; } = CfgStatus.Offline;

        public virtual ICollection<ApplicationConfigurationHistory> ConfigurationHistories { get; private set; }


        public void UpdateValue(string newValue)
        {
            if (CfgValue == newValue) return;
            //string oldValue = CfgValue;
            AddHistory();
            CfgValue = newValue;
            Status = CfgStatus.Offline;
        }

        private void AddHistory()
        {
            if (ConfigurationHistories == null)
                throw new ApplicationException("You must first retrieve this configuration history list.");
            ConfigurationHistories.Add(ApplicationConfigurationHistory.Create(CfgKey, CfgValue));
        }

        public void RevertFromHistory(string historyId)
        {
            if (ConfigurationHistories == null)
                throw new ApplicationException("You must first retrieve this configuration history list.");
            var history = ConfigurationHistories.FirstOrDefault(c => c.Id == historyId);
            UpdateValue(history.CfgValue);
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
