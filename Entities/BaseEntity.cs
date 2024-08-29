namespace ShopAppAPI.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateOnly CreatedDate { get; set; }
        public DateOnly UpdatedDate { get; set; }

    }
}
