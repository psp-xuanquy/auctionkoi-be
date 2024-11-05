using KoiAuction.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using KoiAuction.Domain.Entities;
using Domain.Entities;

namespace KoiAuction.Infrastructure.Persistences;

public class ApplicationDbContext : IdentityDbContext<UserEntity, IdentityRole, string>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<KoiEntity> Kois { get; set; }
    public DbSet<KoiImageEntity> KoiImages { get; set; }
    public DbSet<AuctionHistory> AuctionHistories { get; set; }
    public DbSet<AuctionMethodEntity> AuctionMethods { get; set; }
    public DbSet<AutoBidEntity> AutoBids { get; set; }
    public DbSet<BidEntity> Bids { get; set; } 
    public DbSet<KoiBreederEntity> Breeders { get; set; }
    public DbSet<BlogEntity> Blogs { get; set; } 
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // AspNetUser - Unique Email
        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // KoiEntity
        modelBuilder.Entity<KoiEntity>()
            .HasMany(k => k.Bids)
            .WithOne(b => b.Koi)
            .HasForeignKey(b => b.KoiID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<KoiEntity>()
            .HasMany(k => k.AutoBids)
            .WithOne(ab => ab.Kois)
            .HasForeignKey(ab => ab.KoiID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<KoiEntity>()
            .HasMany(k => k.KoiImages)
            .WithOne(km => km.Koi)
            .HasForeignKey(km => km.KoiID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<KoiEntity>()
            .HasMany(k => k.Transactions)
            .WithOne(t => t.Koi)
            .HasForeignKey(t => t.KoiID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<KoiEntity>()
            .HasOne(k => k.AuctionMethod)
            .WithMany()
            .HasForeignKey(k => k.AuctionMethodID)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<KoiEntity>()
            .HasOne(k => k.Breeder)
            .WithMany()
            .HasForeignKey(k => k.BreederID)
            .OnDelete(DeleteBehavior.Restrict);

        // BidEntity
        modelBuilder.Entity<BidEntity>()
            .HasOne(b => b.Bidder)
            .WithMany(u => u.Bids)
            .HasForeignKey(b => b.BidderID)
            .OnDelete(DeleteBehavior.Restrict);

        // AutoBidEntity
        modelBuilder.Entity<AutoBidEntity>()
            .HasOne(ab => ab.Bidder)
            .WithMany(u => u.AutoBids)
            .HasForeignKey(ab => ab.BidderID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AutoBidEntity>()
            .HasOne(ab => ab.Kois)
            .WithMany(k => k.AutoBids)
            .HasForeignKey(ab => ab.KoiID)
            .OnDelete(DeleteBehavior.Restrict);

        // BlogEntity
        modelBuilder.Entity<BlogEntity>()
            .HasOne(b => b.Author)
            .WithMany(u => u.Blogs)
            .HasForeignKey(b => b.AuthorID)
            .OnDelete(DeleteBehavior.Restrict);

        // AuctionHistory
        modelBuilder.Entity<AuctionHistory>()
            .HasMany(ah => ah.Transactions)
            .WithOne(t => t.AuctionHistory)
            .HasForeignKey(t => t.AuctionHistoryId)
            .OnDelete(DeleteBehavior.Restrict);

        //Transaction
        modelBuilder.Entity<TransactionEntity>()
            .HasOne(t => t.Koi)
            .WithMany(k => k.Transactions)
            .HasForeignKey(t => t.KoiID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TransactionEntity>()
            .HasOne(t => t.AuctionHistory)
            .WithMany(ah => ah.Transactions)
            .HasForeignKey(t => t.AuctionHistoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // KoiBreederEntity
        modelBuilder.Entity<KoiBreederEntity>()
            .HasOne(kb => kb.User)
            .WithMany(u => u.KoiBreeders)
            .HasForeignKey(kb => kb.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private void ConfigureModel(ModelBuilder modelBuilder)
    {


    }
}
