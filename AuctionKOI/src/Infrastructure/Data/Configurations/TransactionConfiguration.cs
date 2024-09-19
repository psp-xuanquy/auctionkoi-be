using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AuctionKOI.Domain.Entities;

namespace AuctionKOI.Infrastructure.Data.Configurations;
public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transaction");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.CommissionRate).IsRequired();
        builder.Property(t => t.TotalAmount).IsRequired();
        builder.Property(t => t.TransactionDate).IsRequired();
        builder.Property(t => t.TransactionStatus).IsRequired();
        builder.Property(t => t.ShippingStatus).IsRequired();
        builder.Property(t => t.CommissionRate).HasColumnType("decimal(18,2)");
        builder.Property(t => t.TotalAmount).HasColumnType("decimal(18,2)");

        builder.HasOne(t => t.Koi)
               .WithMany(k => k.Transactions)
               .HasForeignKey(t => t.KoiID)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Buyer)
               .WithMany(u => u.BoughtTransactions)
               .HasForeignKey(t => t.BuyerID)
               .OnDelete(DeleteBehavior.Restrict); 

        builder.HasOne(t => t.Seller)
               .WithMany(u => u.SoldTransactions)
               .HasForeignKey(t => t.SellerID)
               .OnDelete(DeleteBehavior.Restrict); 
    }
}
