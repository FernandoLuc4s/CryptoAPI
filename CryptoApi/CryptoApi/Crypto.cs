using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoApi
{
    public class Crypto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Site { get; set; }

        public bool IsValid()
        {
            var valid = !string.IsNullOrWhiteSpace(Code);
            valid &= !string.IsNullOrWhiteSpace(Name);
            valid &= !string.IsNullOrWhiteSpace(Site);

            return valid;
        }
    }
}
