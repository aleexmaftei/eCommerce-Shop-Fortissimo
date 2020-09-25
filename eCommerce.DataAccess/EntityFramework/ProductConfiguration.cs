using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.DataAccess.EntityFramework
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> modelBuilder)
        {
            modelBuilder.ToTable("Product");

            modelBuilder.HasIndex(e => e.ProductName)
                    .HasName("UQ__Product__DD5A978AFD9C91CB")
                    .IsUnique();

            modelBuilder.Property(e => e.ProductImage).IsRequired();

            modelBuilder.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Property(e => e.Quantity).HasDefaultValueSql("((1))");
        }
    }
}
