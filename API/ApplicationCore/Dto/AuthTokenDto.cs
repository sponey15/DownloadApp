﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Dto
{
    public class AuthTokenDto : PaginationParams
    {
        public string? AccessToken { get; set; }
        public DateTime? ExpirationTime { get; set; }
    }
}
