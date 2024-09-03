namespace ShopAppAPI.Apps.AdminApp.Dtos.CategoryDto
{
    public class CategoryReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly CreatedDate { get; set; }
        public DateOnly UpdateDate { get; set; }
        public string ImageUrl { get; set; }
        public int ProductsCount { get; set; }
    }
}
