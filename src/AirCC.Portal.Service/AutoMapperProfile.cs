using AirCC.Portal.AppService.ApplicationDtos;
using AirCC.Portal.Domain;
using AutoMapper;

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

            #region Output
            CreateMap<Application, ApplicationListOutput>();
            CreateMap<Application, ApplicationInput>();
            #endregion

        }
    }
}
