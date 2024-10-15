using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums;
public enum AuctionStatus
{
    [EnumMember(Value = "NotStarted")]
    NotStarted,
    [EnumMember(Value = "On-going")]
    OnGoing,
    [EnumMember(Value = "Ended")]
    Ended
}

