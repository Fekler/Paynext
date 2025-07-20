using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paynext.Domain.Validations
{
    public class DomainExceptionValidation : ArgumentException
    {
        internal string Error { get; set; }

        internal DomainExceptionValidation(string error) : base(error) => Error = error;

    }
}
