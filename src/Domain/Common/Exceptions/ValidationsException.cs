using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Common.Exceptions
{
    public class ValidationsException : Exception
    {
        public ValidationsException(string message) : base(message) { }
    }
}
