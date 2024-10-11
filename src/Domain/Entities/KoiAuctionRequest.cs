using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.KoiAuctionRequest;
public class KoiAuctionRequest
{
    [NotMapped]
    public required string BreederId { get; set; }
    public required string KoiId { get; set; }
    public decimal InitialPrice { get; set; }
    public bool AllowAutoBid { get; set; }
    public required string AuctionMethodId { get; set; }
    public bool IsInspectionRequired { get; set; }
}

