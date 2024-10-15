using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums;
public enum AuctionRequestStatus
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "Approved")]
    Approved,
    [EnumMember(Value = "Denied")]
    Denied
}
