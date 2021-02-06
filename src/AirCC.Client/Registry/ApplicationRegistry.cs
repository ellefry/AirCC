﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Client.Registry
{
    public class ApplicationRegistry
    {
        public string Id { get; set; }
        public string Url { get; set; }

        public override bool Equals(object obj)
        {
            var instance = obj as ApplicationRegistry;
            return instance?.Id == Id && instance.Url == Url;
        }
    }
}
