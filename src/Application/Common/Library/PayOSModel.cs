﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Library;
public class PayOsModel
{
    public int orderCode { get; set; }
    public float amount { get; set; }
    public string description { get; set; }
    public string returnUrl { get; set; }
    public string cancelUrl { get; set; }
}
