using AutoMapper;
using ShopAppAPI.Apps.AdminApp.Dtos.CategoryDto;
using ShopAppAPI.Apps.AdminApp.Dtos.ProductDto;
using ShopAppAPI.Entities;
namespace ShopAppAPI.Profiles
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryReturnDto>()
                .ForMember(dest => dest.ImageUrl, map => map.MapFrom(src => "http://localhost:51012/images" + src.Image));
                //.ForMember(dest=>dest.ProductCount, map=>map.MapFrom(src=>src.Products.Count)).ReverseMap();
            CreateMap<Product, ProductReturnDto>();
            CreateMap<Category, CategoryInProductReturnDto>();
        }
    }
}
