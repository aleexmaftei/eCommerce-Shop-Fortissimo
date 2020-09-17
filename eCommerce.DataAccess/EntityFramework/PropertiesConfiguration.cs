using eCommerce.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Data.EntityFramework
{
    class PropertiesConfiguration : IEntityTypeConfiguration<Properties>
    {
        public void Configure(EntityTypeBuilder<Properties> modelBuilder)
        {
            modelBuilder.ToTable("Properties");

            modelBuilder.HasKey(e => e.PropertyId)
                    .HasName("PK__Properti__70C9A7352227DE00");

            modelBuilder.HasIndex(e => e.PropertyName)
                .HasName("UQ__Properti__FDF7CF1BBC37D5C7")
                .IsUnique();

            modelBuilder.Property(e => e.PropertyName)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}
