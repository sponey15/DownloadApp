using ApplicationCore.Dto;
using ApplicationCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Web.Extensions;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadController : ControllerBase
    {
        private readonly IDownloadService _downloadService;
        public DownloadController(IDownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        [HttpGet]
        public async Task<ActionResult<ResourcePathsForReturnDto>> DownloadFile([FromQuery]AuthTokenDto authTokenDto)
        {
            var resources = await _downloadService.GetResourcesListAsync(authTokenDto);
            if (resources == null) return BadRequest("Downloading file failed.");

            Response.AddPaginationHeader(resources.ResourcePathsList.CurrentPage, resources.ResourcePathsList.PageSize,
                                         resources.ResourcePathsList.TotalCount, resources.ResourcePathsList.TotalPages);

            return Ok(resources);
        }
    }
}
