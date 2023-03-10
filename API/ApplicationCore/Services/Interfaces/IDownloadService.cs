using ApplicationCore.Dto;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services.Interfaces
{
    public interface IDownloadService
    {
        Task<ResourcePathsForReturnDto> GetResourcesListAsync(AuthTokenDto authTokenDto);
    }
}
