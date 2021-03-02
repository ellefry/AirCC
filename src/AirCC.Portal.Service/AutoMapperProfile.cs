using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using AirCC.Portal.AppService.ApplicationDtos;
using AirCC.Portal.Domain;

namespace AirCC.Portal.AppService
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Input
            CreateMap<ApplicationInput, Application>();
            CreateMap<CreateConfigurationInput, ApplicationConfiguration>();
            CreateMap<ApplicationConfiguration, ConfigurationListOutput>();
            #endregion


        }
    }
}
