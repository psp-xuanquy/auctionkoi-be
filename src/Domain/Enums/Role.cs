using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums;
public enum Role
{
    [EnumMember(Value = "Manager")]
    MANAGER,
    [EnumMember(Value = "Staff")]
    STAFF,
    [EnumMember(Value = "KoiBreeder")]
    KOIBREEDER,
    [EnumMember(Value = "Customer")]
    CUSTOMER  
}
