﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Enums
{
    public enum Sex
    {
       [EnumMember(Value = "Male")]
       Male,
       [EnumMember(Value = "Female")]
       Female 
    }
}
