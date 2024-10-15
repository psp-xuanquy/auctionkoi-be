using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Enums
{
    public enum Variety
    {
        [EnumMember(Value = "Kohaku")]
        Kohaku,
        [EnumMember(Value = "Ogon")]
        Ogon,
        [EnumMember(Value = "Showa")]
        Showa,
        [EnumMember(Value = "Tancho")]
        Tancho,
        [EnumMember(Value = " Bekko")]
        Bekko,
        [EnumMember(Value = "Doitsu")]
        Doitsu,
        [EnumMember(Value = "Ginrin")]
        Ginrin,
        [EnumMember(Value = "Goshiki")]
        Goshiki,
        [EnumMember(Value = "Benigoi")]
        Benigoi,
        [EnumMember(Value = "Asagi")]
        Asagi,
        [EnumMember(Value = "Platinum")]
        Platinum,
        [EnumMember(Value = "Shusui")]
        Shusui
    }
}
