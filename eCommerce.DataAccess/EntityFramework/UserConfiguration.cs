using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.DataAccess.EntityFramework
{
    class UserConfiguration : IEntityTypeConfiguration<UserT>
    {
        public void Configure(EntityTypeBuilder<UserT> modelBuilder)
        {
            modelBuilder.ToTable("UserT");

            modelBuilder.HasKey(e => e.UserId)
                    .HasName("PK__UserT__1788CC4C57C8650B");

            modelBuilder.HasIndex(e => e.Email)
                .HasName("UQ__UserT__A9D1053475A33B86")
                .IsUnique();

            modelBuilder.Property(e => e.AddressDetail).IsRequired();

            modelBuilder.Property(e => e.CityName)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Property(e => e.CountryName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(35);

            modelBuilder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(35);

            modelBuilder.Property(e => e.PasswordHash).IsRequired();

            modelBuilder.Property(e => e.RegionName)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Property(e => e.PostalCode)
                        .IsRequired();
        }
    }
}
