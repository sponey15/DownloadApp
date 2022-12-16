using ApplicationCore.Dto;
using ApplicationCore.Services.Interfaces;
using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ApplicationCore.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly IMapper _mapper;
        private readonly IAssecoPortalClient _assecoPortalClient;
        private readonly ILogger<DownloadService> _logger;

        public DownloadService(IMapper mapper, IAssecoPortalClient assecoPortalClient, ILogger<DownloadService> logger)
        {
            _mapper = mapper;
            _assecoPortalClient = assecoPortalClient;
            _logger = logger;
        }

        public async Task<ResourcePathsForReturnDto> GetResourcesListAsync(AuthTokenDto authTokenDto)
        {
            authTokenDto = await RefreshAccessTokenAsync(authTokenDto);
            if (authTokenDto == null) return null;

            var file = await _assecoPortalClient.DownloadFileAsync(authTokenDto.AccessToken);
            if (file == null) return null;

            var resourcePathsList = await GetResourcePathsList(file, authTokenDto);
            if (resourcePathsList == null) return null;

            return new ResourcePathsForReturnDto
            {
                ResourcePathsList = resourcePathsList,
                AuthTokenData = authTokenDto
            };
        }

        private async Task<AuthTokenDto> RefreshAccessTokenAsync(AuthTokenDto authTokenDto)
        {
            if (authTokenDto.AccessToken == null || (authTokenDto.ExpirationTime == null || authTokenDto.ExpirationTime < DateTime.Now))
            {
                authTokenDto = _mapper.Map(await _assecoPortalClient.DownloadTokenAsync(), authTokenDto);

                if (authTokenDto == null || authTokenDto.AccessToken == null || authTokenDto.ExpirationTime == null)
                {
                    _logger.LogError($"Error occured while downloading token.");
                    return null;
                }
            }

            return authTokenDto;
        }

        private async Task<PagedList<string>> GetResourcePathsList(string file, AuthTokenDto authTokenDto)
        {
            var parsedFile = XDocument.Parse(file);
            var resourcePaths = parsedFile?.Root?.Descendants()
                                                 .Where(x => x.Name.ToString().Contains("resource"))
                                                 .Attributes()
                                                 .Where(y => !y.Value.StartsWith("/") && !y.Value.StartsWith("http"))
                                                 .Select(z => z.Value)
                                                 .AsQueryable();

            var resourcePathsList = PagedList<string>.Create(resourcePaths, authTokenDto.PageNumber, authTokenDto.PageSize);

            if (resourcePathsList == null) return null;

            return resourcePathsList;
        }
    }
}
