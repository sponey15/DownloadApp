using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class AuthToken
    {
        public string AccessToken { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
