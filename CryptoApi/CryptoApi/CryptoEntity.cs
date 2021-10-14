using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CryptoApi
{
    public class CryptoEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Site { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}