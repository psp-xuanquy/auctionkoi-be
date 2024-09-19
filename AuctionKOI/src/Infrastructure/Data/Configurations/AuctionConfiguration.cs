using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionKOI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuctionKOI.Infrastructure.Data.Configurations;
public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.ToTable("Auction");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Status).IsRequired();

        builder.HasMany(a => a.Kois)
               .WithOne(k => k.Auction)
               .HasForeignKey(k => k.AuctionID)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
