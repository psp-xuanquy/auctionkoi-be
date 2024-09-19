using System.Reflection;
using System.Reflection.Emit;
using AuctionKOI.Application.Common.Interfaces;
using AuctionKOI.Domain.Entities;
using AuctionKOI.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuctionKOI.Infrastructure.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<Koi> Kois => Set<Koi>();

    public DbSet<KoiMedia> KoiMedias => Set<KoiMedia>();

    public DbSet<Auction> Auctions => Set<Auction>();

    public DbSet<Bid> Bids => Set<Bid>();

    public DbSet<AutoBid> AutoBids => Set<AutoBid>();

    public DbSet<Blog> Blogs => Set<Blog>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<Bill> Bills => Set<Bill>();

    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<ApplicationUser>()
        .Property(a => a.Balance)
        .HasPrecision(18, 2);
    }
}
