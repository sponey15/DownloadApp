using AutoMapper;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Moq;

namespace IntegrationTests
{
    public class AssecoPortalClientTests
    {
        [Fact]
        public async Task DownloadToken_If_Success_Returns_AuthToken()
        {
            //Arange
            var config = Mock.Of<IConfiguration>();

            Mock.Get(config).Setup(x => x["OauthClientUtl"])
                .Returns("https://oauth2.assecobs.pl/api/oauth2/token");
            Mock.Get(config).Setup(x => x["OauthUsername"])
                .Returns("PortalCloudAPI_F1645EDF0ABE4CBBA9D5135314117619");
            Mock.Get(config).Setup(x => x["OauthPassword"])
                .Returns("sV7fO40gNd3v9x48beVq42zy07Mv6UJB");
            Mock.Get(config).Setup(x => x["AssecoPortalUrl"])
                .Returns("https://portalcloudapi-test.assecobs.pl/?DBC=rest&wadl");

            //Act
            var assecoPortalClient = new AssecoPortalClient(config);

            var result = await assecoPortalClient.DownloadTokenAsync();

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DownloadToken_With_Wrong_Credentials_Returns_Null()
        {
            //Arange
            var config = Mock.Of<IConfiguration>();

            Mock.Get(config).Setup(x => x["OauthClientUtl"])
                .Returns("https://oauth2.assecobs.pl/api/oauth2/token");
            Mock.Get(config).Setup(x => x["OauthUsername"])
                .Returns("wrong credentials");
            Mock.Get(config).Setup(x => x["OauthPassword"])
                .Returns("wrong credentials");
            Mock.Get(config).Setup(x => x["AssecoPortalUrl"])
                .Returns("https://portalcloudapi-test.assecobs.pl/?DBC=rest&wadl");

            //Act
            var assecoPortalClient = new AssecoPortalClient(config);

            var result = await assecoPortalClient.DownloadTokenAsync();

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DownloadFile_If_Success_Returns_File()
        {
            //Arange
            var config = Mock.Of<IConfiguration>();

            Mock.Get(config).Setup(x => x["OauthClientUtl"])
                .Returns("https://oauth2.assecobs.pl/api/oauth2/token");
            Mock.Get(config).Setup(x => x["OauthUsername"])
                .Returns("PortalCloudAPI_F1645EDF0ABE4CBBA9D5135314117619");
            Mock.Get(config).Setup(x => x["OauthPassword"])
                .Returns("sV7fO40gNd3v9x48beVq42zy07Mv6UJB");
            Mock.Get(config).Setup(x => x["AssecoPortalUrl"])
                .Returns("https://portalcloudapi-test.assecobs.pl/?DBC=rest&wadl");

            //Act
            var assecoPortalClient = new AssecoPortalClient(config);

            var token = await assecoPortalClient.DownloadTokenAsync();
            var result = await assecoPortalClient.DownloadFileAsync(token.AccessToken);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DownloadFile_If_Fail_Returns_Null()
        {
            //Arange
            var config = Mock.Of<IConfiguration>();

            Mock.Get(config).Setup(x => x["OauthClientUtl"])
                .Returns("https://oauth2.assecobs.pl/api/oauth2/token");
            Mock.Get(config).Setup(x => x["OauthUsername"])
                .Returns("wrong credentials");
            Mock.Get(config).Setup(x => x["OauthPassword"])
                .Returns("wrong credentials");
            Mock.Get(config).Setup(x => x["AssecoPortalUrl"])
                .Returns("https://portalcloudapi-test.assecobs.pl/?DBC=rest&wadl");

            //Act
            var assecoPortalClient = new AssecoPortalClient(config);

            var result = await assecoPortalClient.DownloadFileAsync(string.Empty);

            //Assert
            Assert.Null(result);
        }
    }
}