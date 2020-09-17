using eCommerce.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Data.EntityFramework
{
    class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> modelBuilder)
        {
            modelBuilder.ToTable("ProductCategory");

            modelBuilder.HasIndex(e => e.ProductCategoryName)
                     .HasName("UQ__ProductC__CE9F88B552EA48E7")
                     .IsUnique();

            modelBuilder.Property(e => e.ProductCategoryImage).IsRequired();

            modelBuilder.Property(e => e.ProductCategoryName)
                .IsRequired()
                .HasMaxLength(35);

            modelBuilder.HasOne(d => d.ParentProductCategory)
                .WithMany(p => p.InverseParentProductCategory)
                .HasForeignKey(d => d.ParentProductCategoryId)
                .HasConstraintName("FK__ProductCa__Paren__7C6F7215");
        }
    }
}
