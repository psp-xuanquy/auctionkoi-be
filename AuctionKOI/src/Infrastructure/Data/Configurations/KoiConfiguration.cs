using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionKOI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuctionKOI.Infrastructure.Data.Configurations;
public class KoiConfiguration : IEntityTypeConfiguration<Koi>
{
    public void Configure(EntityTypeBuilder<Koi> builder)
    {
        builder.ToTable("Koi");
        builder.HasKey(k => k.Id);

        builder.Property(k => k.Name).HasMaxLength(255);
        builder.Property(k => k.Length).IsRequired();
        builder.Property(k => k.Age).IsRequired();
        builder.Property(k => k.InitialPrice).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(k => k.Rating).IsRequired();
        builder.Property(k => k.Description).HasMaxLength(1000);
        builder.Property(k => k.ImageUrl).HasMaxLength(500);

        builder.HasOne(k => k.Auction)
               .WithMany(a => a.Kois)
               .HasForeignKey(k => k.AuctionID)
               .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(k => k.Breeder)
               .WithMany(u => u.Kois)
               .HasForeignKey(k => k.BreederID)
               .OnDelete(DeleteBehavior.Restrict); 
    }
}

