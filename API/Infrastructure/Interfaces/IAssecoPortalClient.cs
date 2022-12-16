using Infrastructure.Entities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IAssecoPortalClient
    {
        Task<AuthToken> DownloadTokenAsync();
        Task<string> DownloadFileAsync(string authToken);
    }
}
