using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums;
public enum TransactionType
{
    [EnumMember(Value = "Bought")]
    Bought,
    [EnumMember(Value = "Sold")]
    Sold
}
