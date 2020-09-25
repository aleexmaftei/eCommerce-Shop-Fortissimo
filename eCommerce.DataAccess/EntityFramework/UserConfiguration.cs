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
                     .HasName("PK__UserT__1788CC4C0B62B2EE");

            modelBuilder.HasIndex(e => e.Email)
                .HasName("UQ__UserT__A9D105349376AADF")
                .IsUnique();

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
        }
    }
}
