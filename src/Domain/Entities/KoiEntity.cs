using Domain.Enums;
using KoiAuction.Domain.Entities.Base;
using KoiAuction.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Entities
{
    [Table("Koi")]
    public class KoiEntity : BaseEntity
    { 
        public string? Name { get; set; }
        public Sex Sex { get; set; }
        public double Size { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public Variety Variety { get; set; }
        public decimal InitialPrice { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? RequestResponse { get; set; }
        public int? DescendingRate { get; set; }
        public decimal? LowestDescendedPrice { get; set; }
        public int? DescendingTimeInMinute { get; set; }
        public AuctionRequestStatus AuctionRequestStatus { get; set; }
        public AuctionStatus AuctionStatus { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool AllowAutoBid { get; set; }

        [ForeignKey("AuctionMethodID")]
        public string? AuctionMethodID { get; set; }
        public virtual AuctionMethodEntity? AuctionMethod { get; set; }

        [ForeignKey("BreederID")]
        public string? BreederID { get; set; }
        public virtual UserEntity? Breeder { get; set; }

        public virtual ICollection<AutoBidEntity>? AutoBids { get; set; }
        public virtual ICollection<BidEntity>? Bids { get; set; }
        public virtual ICollection<KoiImageEntity>? KoiImages { get; set; }
        public virtual ICollection<TransactionEntity>? Transactions { get; set; }

        public KoiEntity()
        {
            AuctionStatus = AuctionStatus.NotStarted;
            AutoBids = new List<AutoBidEntity>();
            Bids = new List<BidEntity>();
            KoiImages = new List<KoiImageEntity>();
            Transactions = new List<TransactionEntity>();
        }

        public KoiEntity(decimal initialPrice, string name, string auctionMethodId) : this() 
        {
            if (initialPrice < 50)
                throw new ArgumentException("Price must be at least $50.");

            InitialPrice = initialPrice;
            Name = name;
            AuctionMethodID = auctionMethodId;
        }

        public void StartAuction()
        {
            AuctionStatus = AuctionStatus.OnGoing;
            StartTime = DateTime.Now;
            EndTime = StartTime?.AddMinutes(1);
        }

        public void EndAuction()
        {
            AuctionStatus = AuctionStatus.Ended;
        }

        public bool IsAuctionExpired()
        {
            return EndTime.HasValue && DateTime.Now >= EndTime.Value;
        }
    }
}
