using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.DataAccess.EntityFramework
{
    public class UserInvoiceConfiguration : IEntityTypeConfiguration<UserInvoice>
    {
        public void Configure(EntityTypeBuilder<UserInvoice> modelBuilder)
        {
            modelBuilder.ToTable("UserInvoice");

            modelBuilder.Property(e => e.DateBuy)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

            modelBuilder.Property(e => e.QuantityBuy).HasDefaultValueSql("((1))");

            modelBuilder.HasOne(d => d.DeliveryLocation)
                .WithMany(p => p.UserInvoice)
                .HasForeignKey(d => d.DeliveryLocationId)
                .HasConstraintName("FK__UserInvoi__Deliv__7A521F79");

            modelBuilder.HasOne(d => d.Product)
                .WithMany(p => p.UserInvoice)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__UserInvoi__Produ__795DFB40");
        }
    }
}
