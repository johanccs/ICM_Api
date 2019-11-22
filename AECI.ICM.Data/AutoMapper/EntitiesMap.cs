using AECI.ICM.Domain.Entities;
using AutoMapper;

namespace AECI.ICM.Data.AutoMapper
{
    public class EntitiesMap: Profile
    {
        public IMapper Create()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Entities.ICM, ICMEntity>();
                cfg.CreateMap<Entities.SectionDetail, SectionDetail>();
            });

            return config.CreateMapper();
        }
    }
}
