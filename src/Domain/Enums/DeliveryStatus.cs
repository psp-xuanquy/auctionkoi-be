using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Enums
{
    public enum DeliveryStatus
    {
        [EnumMember(Value = "On-Going")]
        OnGoing,
        [EnumMember(Value = "Finished")]
        Finished
    }
}
