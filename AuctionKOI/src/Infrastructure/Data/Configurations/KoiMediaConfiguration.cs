using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionKOI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuctionKOI.Infrastructure.Data.Configurations;
public class KoiMediaConfiguration : IEntityTypeConfiguration<KoiMedia>
{
    public void Configure(EntityTypeBuilder<KoiMedia> builder)
    {
        builder.ToTable("KoiMedia");
        builder.HasKey(km => km.Id);

        builder.Property(km => km.Url).HasMaxLength(500);
        builder.Property(km => km.UrlType).HasMaxLength(100);

        builder.HasOne(km => km.Koi)
               .WithMany(k => k.KoiMedias)
               .HasForeignKey(km => km.KoiID)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
