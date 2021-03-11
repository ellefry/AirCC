using System;
using System.IO;

namespace AirCC.Client
{
    public class AirCCConfigOptions
    {
        public const string SectionName = "AirCC";
        public const string DefaultFileName = "AirCC.Settings.ini";
        public string FilePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FileName))
                    FileName = AirCCConfigOptions.DefaultFileName;
                if (!string.IsNullOrWhiteSpace(MainPath))
                    return Path.Combine(MainPath, FileName);
                else
                    return Path.Combine(Environment.CurrentDirectory, FileName);
            }
        }
        public string MainPath { get; set; }
        public string FileName { get; set; }

        /// <summary>
        /// Application host url
        /// </summary>
        public string PublicOrigin { get; set; }
        /// <summary>
        /// AirCC service url
        /// </summary>
        public string RegistryServiceUrl { get; set; }
        /// <summary>
        /// Application id from AirCC
        /// </summary>
        public string ApplicationId { get; set; }
        public string ApplicationSecret { get; set; }

    }
}
