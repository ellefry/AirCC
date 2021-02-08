﻿using AirCC.Client.Registry;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Portal.Service
{
    public class ApplicationRegistryAppSevice : IApplicationRegistryAppService
    {
        private readonly IMemoryCache memoryCache;

        public ApplicationRegistryAppSevice(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public void AddApplication([NotNull]ApplicationRegistry applicationRegistry)
        {
            if (memoryCache.TryGetValue(applicationRegistry.Id, out ApplicationRegistry val))
            {
                if (!val.Equals(applicationRegistry))
                {
                    memoryCache.Set(applicationRegistry.Id, applicationRegistry, TimeSpan.FromMinutes(20));
                }
            }
            else
            {
                memoryCache.Set(applicationRegistry.Id, applicationRegistry, TimeSpan.FromMinutes(20));
            }
        }
    }
}