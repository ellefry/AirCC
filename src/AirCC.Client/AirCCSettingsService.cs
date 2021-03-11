using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AirCC.Client
{
    public interface IAirCCSettingsService
    {
        Task Update(IEnumerable<AirCCSetting> airCCSettings);
    }

    public class AirCCSettingsService : IAirCCSettingsService
    {
        AirCCConfigOptions airCCConfigOptions;

        public AirCCSettingsService(AirCCConfigOptions airCCConfigOptions)
        {
            this.airCCConfigOptions = airCCConfigOptions;
        }

        /// <summary>
        /// Update local setttings
        /// </summary>
        /// <param name="airCCSettings"></param>
        /// <returns></returns>
        public async Task Update(IEnumerable<AirCCSetting> airCCSettings)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var setting in airCCSettings)
            {
                stringBuilder.Append($"{setting.Key}={setting.Value}{Environment.NewLine}");
            }
            File.WriteAllText(airCCConfigOptions.FilePath, stringBuilder.ToString());
            await Task.CompletedTask;
        }
    }
}
