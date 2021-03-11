using BCI.Extensions.Entity;

namespace AirCC.Portal.Domain
{
    public class ApplicationConfigurationHistory : FullAuditEntity<string>
    {
        public string CfgKey { get; set; }
        public string CfgValue { get; set; }

        public static ApplicationConfigurationHistory Create(string key, string value)
        {
            return new ApplicationConfigurationHistory
            {
                CfgKey = key,
                CfgValue = value
            };
        }
    }
}
