namespace ShopAppAPI.Apps.AdminApp.Dtos.ProductDto
{
    public class ProductItemListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double SalePrice { get; set; }
        public double CostPrice { get; set; }
        public DateOnly CreatedDate { get; set; }
        public DateOnly UpdatedDate { get; set; }
        public string CategoryName {  get; set; }   
    }
}
