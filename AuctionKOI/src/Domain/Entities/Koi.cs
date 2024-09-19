using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AuctionKOI.Domain.Entities;

[Table("Koi")]
public class Koi : BaseAuditableEntity
{
    public required string AuctionID { get; set; }
    public required string BreederID { get; set; }
    public string? Name { get; set; }
    public Sex Sex { get; set; }
    public double Length { get; set; }
    public int Age { get; set; }
    public decimal InitialPrice { get; set; }
    public double Rating { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    [ForeignKey("AuctionID")]
    public virtual Auction? Auction { get; set; }

    [ForeignKey("BreederID")]
    public virtual ApplicationUser? Breeder { get; set; }

    public virtual ICollection<AutoBid>? AutoBids { get; set; } 
    public virtual ICollection<Bid>? Bids { get; set; } 

    public virtual ICollection<KoiMedia>? KoiMedias { get; set; }
    public virtual ICollection<Transaction>? Transactions { get; set; }
}

