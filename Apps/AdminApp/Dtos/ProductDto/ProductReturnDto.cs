namespace ShopAppAPI.Apps.AdminApp.Dtos.ProductDto
{
    public class ProductReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double SalePrice { get; set; }
        public double CostPrice { get; set; }
        public DateOnly CreatedDate { get; set; }
        public DateOnly UpdatedDate { get; set; }
        public CategoryInProductReturnDto Category {  get; set; }
    }
    public class CategoryInProductReturnDto
    {
        public string Name { get; set; }
        public int ProductsCount { get; set; }
    }
}
