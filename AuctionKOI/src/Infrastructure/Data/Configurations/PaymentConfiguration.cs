using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionKOI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuctionKOI.Infrastructure.Data.Configurations;
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payment");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.PaymentStatus).IsRequired();
        builder.Property(p => p.PaymentDate).IsRequired();

        builder.HasOne(p => p.Transaction)
               .WithMany(t => t.Payments)
               .HasForeignKey(p => p.TransactionID)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
