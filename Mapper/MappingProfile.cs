using AutoMapper;
using server.Dto;
using server.Entities;

namespace server.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {
            CreateMap<Image, ImageDtoRes>();
            CreateMap<Product, CreateProductReq>().ReverseMap();
            CreateMap<Brand, CreateBrandReq>().ReverseMap();
            CreateMap<Categories, CreateCategoryReq>().ReverseMap();
            CreateMap<CreateProductReq, Product>()
              .ForMember(x => x.Thumbnail, opt => opt.Ignore()); // Image manually set in service
            CreateMap<CreateBrandReq, Brand>()
                .ForMember(x => x.Image, opt => opt.Ignore()); // Image manually set in service
            CreateMap<CreateCategoryReq, Categories>()
                .ForMember(x => x.Image, opt => opt.Ignore()); // I
            CreateMap<Product, ProductResDto>();
            CreateMap<Brand, BrandResDto>();
            CreateMap<Categories, CategoryResDto>();
            CreateMap<WishListItems, WishListitemResDto>();

        }
    }
}
