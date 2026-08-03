using AutoMapper;
using server.Dto;
using server.Enity;

namespace server.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<User, UserDto>();
        }
    }
}
