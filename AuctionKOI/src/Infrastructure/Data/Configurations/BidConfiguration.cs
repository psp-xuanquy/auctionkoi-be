using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionKOI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuctionKOI.Infrastructure.Data.Configurations;
public class BidConfiguration : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.ToTable("Bid");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.BidAmount).IsRequired();
        builder.Property(b => b.BidTime).IsRequired();
        builder.Property(b => b.IsWinningBid).IsRequired();
        builder.Property(b => b.IsLatest).IsRequired();
        builder.Property(b => b.ExpireDate).IsRequired();
        builder.Property(b => b.BidAmount).HasColumnType("decimal(18,2)");

        builder.HasOne(b => b.Koi)
               .WithMany(k => k.Bids)
               .HasForeignKey(b => b.KoiID)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Bidder)
               .WithMany(u => u.Bids)
               .HasForeignKey(b => b.BidderID)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
