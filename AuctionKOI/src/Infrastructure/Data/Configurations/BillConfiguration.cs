using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionKOI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuctionKOI.Infrastructure.Data.Configurations;
public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("Bill");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.TotalAmount).IsRequired();
        builder.Property(b => b.PaymentDate).IsRequired();
        builder.Property(b => b.TotalAmount).HasColumnType("decimal(18,2)");

        builder.HasOne(b => b.Customer)
               .WithMany(u => u.Bills)
               .HasForeignKey(b => b.CustomerID)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
