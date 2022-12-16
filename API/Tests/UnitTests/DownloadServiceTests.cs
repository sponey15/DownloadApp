using ApplicationCore;
using ApplicationCore.Dto;
using ApplicationCore.Services;
using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    public class DownloadServiceTests
    {
        [Fact]
        public async Task GetResourcesList_Without_Authentication_Data_Returns_Resource_List()
        {
            //Arange
            var authTokenDto = new AuthTokenDto();
            var authToken = new AuthToken();
            authToken.AccessToken = "test token";
            authToken.ExpirationTime = DateTime.Now.AddSeconds(3600);
            var testFile = "<application xmlns=\"http://wadl.dev.java.net/2009/02\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:abs=\"http://www.assecobs.pl\">\r\n<grammars>\r\n<xsd:schema xmlns=\"http://www.assecobs.pl\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" targetNamespace=\"http://www.assecobs.pl\" elementFormDefault=\"qualified\"><xsd:element xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" name=\"chr_AbsenceSubTypeDelete\"><xsd:complexType><xsd:sequence><xsd:element xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" name=\"Arraychr_AbsenceSubTypeDeleteData\" type=\"TArraychr_AbsenceSubTypeDeleteData\" minOccurs=\"1\" maxOccurs=\"1\"/></xsd:sequence></xsd:complexType></xsd:element><xsd:complexType xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" name=\"TArraychr_AbsenceSubTypeDeleteData\"><xsd:sequence><xsd:element xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" minOccurs=\"1\" maxOccurs=\"unbounded\" name=\"chr_AbsenceSubTypeDeleteData\" type=\"Tchr_AbsenceSubTypeDeleteData\" nillable=\"false\"/></xsd:sequence></xsd:complexType><xsd:complexType xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" name=\"Tchr_AbsenceSubTypeDeleteData\"><xsd:sequence><xsd:element xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" minOccurs=\"0\" maxOccurs=\"1\" name=\"__id_cloud\" type=\"xsd:long\" nillable=\"true\"/></xsd:sequence></xsd:complexType></xsd:schema>\r\n</grammars>\r\n<resources base=\"https://portalcloudapi-test.assecobs.pl/TEST_API/services_integration_api/ApiWebService.ashx/\">\r\n  <resource path=\"/\">\r\n    <resource path=\"chr_AbsenceSubTypeDelete\">\r\n      <param name=\"DBC\" required=\"true\" type=\"xsd:string\" style=\"query\" default=\"rest\" />\r\n      <method name=\"POST\">\r\n        <request>\r\n          <representation mediaType=\"application/json\" element=\"abs:chr_AbsenceSubTypeDelete\" />\r\n        </request>\r\n        <response>\r\n          <representation mediaType=\"application/json\" element=\"abs:chr_AbsenceSubTypeDeleteResponse\" />\r\n        </response>\r\n      </method>\r\n    </resource>\r\n  </resource>\r\n\r\n  <resource path=\"/\">\r\n    <resource path=\"chr_AbsenceSubTypeGet\">\r\n      <param name=\"DBC\" required=\"true\" type=\"xsd:string\" style=\"query\" default=\"rest\" />\r\n      <method name=\"GET\">\r\n        <request>\r\n          <param name=\"JSON\" required=\"true\" type=\"xsd:string\" style=\"query\" />\r\n          <representation mediaType=\"application/json\" element=\"abs:chr_AbsenceSubTypeGet\" />\r\n        </request>\r\n        <response>\r\n          <representation mediaType=\"application/json\" element=\"abs:chr_AbsenceSubTypeGetResponse\" />\r\n        </response>\r\n      </method>\r\n    </resource>\r\n  </resource>\r\n\r\n  <resource path=\"/\">\r\n    <resource path=\"chr_AbsenceSubTypeModify\">\r\n      <param name=\"DBC\" required=\"true\" type=\"xsd:string\" style=\"query\" default=\"rest\" />\r\n      <method name=\"POST\">\r\n        <request>\r\n          <representation mediaType=\"application/json\" element=\"abs:chr_AbsenceSubTypeModify\" />\r\n        </request>\r\n        <response>\r\n          <representation mediaType=\"application/json\" element=\"abs:chr_AbsenceSubTypeModifyResponse\" />\r\n        </response>\r\n      </method>\r\n    </resource>\r\n  </resource>\r\n</resources>\r\n</application>";

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            var assecoPortalClient = Mock.Of<IAssecoPortalClient>();
            var logger = Mock.Of<ILogger<DownloadService>>();

            Mock.Get(assecoPortalClient).Setup(x => x.DownloadTokenAsync())
                                        .ReturnsAsync(authToken);

            Mock.Get(assecoPortalClient).Setup(x => x.DownloadFileAsync(It.IsAny<string>()))
                                        .ReturnsAsync(testFile);
            //Act
            var downloadService = new DownloadService(mapper, assecoPortalClient, logger);
            var result = await downloadService.GetResourcesListAsync(authTokenDto);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.ResourcePathsList.Count);
        }

        [Fact]
        public async Task GetResourcesList_When_DownloadingToken_Fails_Returns_Null()
        {
            //Arange
            var authTokenDto = new AuthTokenDto();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            var assecoPortalClient = Mock.Of<IAssecoPortalClient>();
            var logger = Mock.Of<ILogger<DownloadService>>();

            Mock.Get(assecoPortalClient).Setup(x => x.DownloadTokenAsync())
                                        .ReturnsAsync((AuthToken)null);

            Mock.Get(assecoPortalClient).Setup(x => x.DownloadFileAsync(It.IsAny<string>()))
                                        .ReturnsAsync((string)null);
            //Act
            var downloadService = new DownloadService(mapper, assecoPortalClient, logger);
            var result = await downloadService.GetResourcesListAsync(authTokenDto);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetResourcesList_When_DownloadFile_Fails_Returns_Null()
        {
            //Arange
            var authTokenDto = new AuthTokenDto();
            var authToken = new AuthToken();
            authToken.AccessToken = "test token";
            authToken.ExpirationTime = DateTime.Now.AddSeconds(3600);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            var assecoPortalClient = Mock.Of<IAssecoPortalClient>();
            var logger = Mock.Of<ILogger<DownloadService>>();

            Mock.Get(assecoPortalClient).Setup(x => x.DownloadTokenAsync())
                                        .ReturnsAsync(authToken);

            Mock.Get(assecoPortalClient).Setup(x => x.DownloadFileAsync(It.IsAny<string>()))
                                        .ReturnsAsync((string)null);
            //Act
            var downloadService = new DownloadService(mapper, assecoPortalClient, logger);
            var result = await downloadService.GetResourcesListAsync(authTokenDto);

            //Assert
            Assert.Null(result);
        }
    }
}