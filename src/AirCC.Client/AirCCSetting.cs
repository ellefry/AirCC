using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Client
{
    [Serializable]
    public class AirCCSetting
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class AirCCSettingCollection
    {
        public List<AirCCSetting> AirCCSettings { get; set; } = new List<AirCCSetting>();
    }
}
