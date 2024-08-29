using ShopAppAPI.Entities;

namespace ShopAppAPI.Apps.AdminApp.Dtos.ProductDto
{
    public class ProductListDto
    {
        public int Page { get; set; }
        public int TotalCount { get; set; } 
        public List<ProductItemListDto> Items { get; set; }
    }
}
