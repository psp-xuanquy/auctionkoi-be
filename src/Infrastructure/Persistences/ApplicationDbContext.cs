using KoiAuction.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using KoiAuction.Domain.Entities;

namespace KoiAuction.Infrastructure.Persistences;

public class ApplicationDbContext : IdentityDbContext<AspNetUser, IdentityRole, string>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<KoiEntity> Kois { get; set; }
    public DbSet<KoiMediaEntity> KoiMedias { get; set; }
    public DbSet<AuctionEntity> Auctions { get; set; }
    public DbSet<AuctionMethodEntity> AuctionMethods { get; set; }
    public DbSet<AutoBidEntity> AutoBids { get; set; }
    public DbSet<BidEntity> Bids { get; set; }
    public DbSet<BillEntity> Bills { get; set; }
    public DbSet<BlogEntity> Blogs { get; set; }
    public DbSet<PaymentEntity> Payments { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<AspNetUser> Users => Set<AspNetUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // AspNetUser - Unique Email
        modelBuilder.Entity<AspNetUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // AuctionEntity
        modelBuilder.Entity<AuctionEntity>()
            .HasMany(a => a.Kois)
            .WithOne(k => k.Auction)
            .HasForeignKey(k => k.AuctionID)
            .OnDelete(DeleteBehavior.Restrict);

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
            .HasMany(k => k.KoiMedias)
            .WithOne(km => km.Koi)
            .HasForeignKey(km => km.KoiID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<KoiEntity>()
            .HasMany(k => k.Transactions)
            .WithOne(t => t.Koi)
            .HasForeignKey(t => t.KoiID)
            .OnDelete(DeleteBehavior.Restrict);

        // TransactionEntity
        modelBuilder.Entity<TransactionEntity>()
            .HasMany(t => t.Payments)
            .WithOne(p => p.Transaction)
            .HasForeignKey(p => p.TransactionID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TransactionEntity>()
            .HasOne(t => t.Buyer)
            .WithMany(u => u.BoughtTransactions)
            .HasForeignKey(t => t.BuyerID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TransactionEntity>()
            .HasOne(t => t.Seller)
            .WithMany(u => u.SoldTransactions)
            .HasForeignKey(t => t.SellerID)
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

        // BillEntity
        modelBuilder.Entity<BillEntity>()
            .HasOne(b => b.Customer)
            .WithMany(u => u.Bills)
            .HasForeignKey(b => b.CustomerID)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private void ConfigureModel(ModelBuilder modelBuilder)
    {


    }
}