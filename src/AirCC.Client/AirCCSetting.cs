using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Client
{
    public class AirCCSetting
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class AirCCSettingCollection
    {
        public List<AirCCSetting> AirCCSettings { get; set; } = new List<AirCCSetting>();
    }
}
