using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.DataAccess.EntityFramework
{
    class UserBuyHistoryConfiguration : IEntityTypeConfiguration<UserBuyHistory>
    {
        public void Configure(EntityTypeBuilder<UserBuyHistory> modelBuilder)
        {
            modelBuilder.ToTable("UserBuyHistory");

            modelBuilder.HasKey(e => e.UserBuyHistoryId)
                    .HasName("PK__UserBuyH__1C6CE9A92F815F38");

            modelBuilder.Property(e => e.UserBuyHistoryId).HasColumnName("UserBuyHistoryId");

            modelBuilder.Property(e => e.DateBuy)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            modelBuilder.Property(e => e.QuantityBuy).HasDefaultValueSql("((1))");

            modelBuilder.HasOne(d => d.Product)
                .WithMany(p => p.UserBuyHistory)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__UserBuyHi__Produ__17236851");

            modelBuilder.HasOne(d => d.User)
                .WithMany(p => p.UserBuyHistory)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserBuyHi__UserI__162F4418");
        }
    }
}
