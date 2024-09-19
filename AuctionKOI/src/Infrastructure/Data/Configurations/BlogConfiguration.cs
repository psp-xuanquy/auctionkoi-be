using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionKOI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuctionKOI.Infrastructure.Data.Configurations;
public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.ToTable("Blog");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title).HasMaxLength(255);
        builder.Property(b => b.Content).IsRequired();
        builder.Property(b => b.PostedDate).IsRequired();
        builder.Property(b => b.Status).IsRequired();

        builder.HasOne(b => b.Author)
               .WithMany(u => u.Blogs)
               .HasForeignKey(b => b.AuthorID)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
