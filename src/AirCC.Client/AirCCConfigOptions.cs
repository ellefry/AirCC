using System;
using System.Collections.Generic;
using System.Text;
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
                    return Path.Combine(Environment.CurrentDirectory,FileName);
            }
        }
        public string MainPath { get; set; }
        public string FileName { get; set; }

    }
}
