using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionKOI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuctionKOI.Infrastructure.Data.Configurations;
public class AutoBidConfiguration : IEntityTypeConfiguration<AutoBid>
{
    public void Configure(EntityTypeBuilder<AutoBid> builder)
    {
        builder.ToTable("AutoBid");
        builder.HasKey(ab => ab.Id);

        builder.Property(ab => ab.MaxBid).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(ab => ab.BidTime).IsRequired();

        builder.HasOne(ab => ab.Kois)
               .WithMany(k => k.AutoBids)
               .HasForeignKey(ab => ab.KoiID)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ab => ab.Bidder)
               .WithMany(u => u.AutoBids)
               .HasForeignKey(ab => ab.BidderID)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
