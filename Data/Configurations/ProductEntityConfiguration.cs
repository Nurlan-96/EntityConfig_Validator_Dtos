using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopAppAPI.Apps.AdminApp.Dtos.ProductDto;
using ShopAppAPI.Entities;

namespace ShopAppAPI.Data.Configurations
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductItemListDto>
    {
        public void Configure(EntityTypeBuilder<ProductItemListDto> builder)
        {
            builder.Property(p => p.Name).IsRequired(true).HasMaxLength(50);
            builder.Property(p => p.SalePrice).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(p=>p.CostPrice).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(p => p.CreatedDate).HasDefaultValueSql("getdate()");
            builder.Property(p => p.UpdatedDate).HasDefaultValueSql("getdate()");
        }
    }
}
