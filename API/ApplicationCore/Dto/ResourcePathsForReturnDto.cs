using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Dto
{
    public class ResourcePathsForReturnDto
    {
        public PagedList<string> ResourcePathsList { get; set; }
        public AuthTokenDto AuthTokenData { get; set; }
    }
}
