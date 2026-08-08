using AutoMapper;
using server.Dto;
using server.Enity;

namespace server.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<RegisterRequest, User>();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<CountryRequestDto, Country>();
        }
    }
}