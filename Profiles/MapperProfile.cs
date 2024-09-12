using AutoMapper;
using ShopAppAPI.Apps.AdminApp.Dtos.CategoryDto;
using ShopAppAPI.Apps.AdminApp.Dtos.ProductDto;
using ShopAppAPI.Apps.AdminApp.Dtos.UserDto;
using ShopAppAPI.Entities;
namespace ShopAppAPI.Profiles
{
    public class MapperProfile:Profile
    {
        private readonly IHttpContextAccessor _contextAccessor; 

        public MapperProfile(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            var urlBuilder = new UriBuilder(_contextAccessor.HttpContext.Request.Scheme,
                _contextAccessor.HttpContext.Request.Host.Host,
                _contextAccessor.HttpContext.Request.Host.Port.Value);
             var url = urlBuilder.Uri.AbsoluteUri;
             
            CreateMap<Category, CategoryReturnDto>()
                .ForMember(dest => dest.ImageUrl, map => map.MapFrom(src => url+"images/" + src.Image));
                //.ForMember(dest=>dest.ProductCount, map=>map.MapFrom(src=>src.Products.Count)).ReverseMap();
            CreateMap<Product, ProductReturnDto>();
            CreateMap<Category, CategoryInProductReturnDto>();
            CreateMap<AppUser, UserGetDto>();
        }
    }
}
