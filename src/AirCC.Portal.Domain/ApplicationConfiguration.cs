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


        //public void AddHistory()
        //{
        //    if (_configurationHistories == null)
        //        throw new ApplicationException("You must first retrieve this configuration history list.");
        //    _configurationHistories.Add(ApplicationConfigurationHistory.Create(CfgKey, CfgValue));
        //}

        public void RevertFromHistory(string historyId)
        {
            if (_configurationHistories == null)
                throw new ApplicationException("You must first retrieve this configuration history list.");
            var history = _configurationHistories.FirstOrDefault(c => c.Id == historyId);
            _configurationHistories.Add(ApplicationConfigurationHistory.Create(CfgKey, CfgValue));
            if (history == null)
                throw new ApplicationException("You must first retrieve this configuration history list.");
            CfgKey = history.CfgKey;
            CfgValue = history.CfgValue;
            Status = CfgStatus.Offline;
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
