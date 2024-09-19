using AuctionKOI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuctionKOI.Domain.Entities;
public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? FarmName { get; set; }
    public string? Address { get; set; }
    public bool Status { get; set; }
    public decimal Balance { get; set; }

    public virtual ICollection<Transaction>? BoughtTransactions { get; set; }
    public virtual ICollection<Transaction>? SoldTransactions { get; set; }
    public virtual ICollection<Bill>? Bills { get; set; }
    public virtual ICollection<Blog>? Blogs { get; set; }
    public virtual ICollection<Koi>? Kois { get; set; }
    public virtual ICollection<Bid>? Bids { get; set; }
    public virtual ICollection<AutoBid>? AutoBids { get; set; }
}
