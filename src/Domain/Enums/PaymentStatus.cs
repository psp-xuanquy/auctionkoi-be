using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Enums
{
    public enum PaymentStatus
    {
        [EnumMember(Value = "Payment successful")]
        Success,
        [EnumMember(Value = "Payment failed")]
        Fail,
        [EnumMember(Value = "Transaction canceled")]
        Cancel,
    }
}
