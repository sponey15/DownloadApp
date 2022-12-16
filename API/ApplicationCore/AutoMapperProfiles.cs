using ApplicationCore.Dto;
using AutoMapper;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ApplicationCore
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AuthToken, AuthTokenDto>().ForMember(x => x.PageNumber, opt => opt.Ignore())
                                                .ForMember(x => x.PageSize, opt => opt.Ignore());
        }
    }
}
