using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitchChatDownloader.Config;
using TwitchChatDownloader.Extensions;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Writers.SrtFile;
using TwitchChatDownloader.Writers.SrtLines;

namespace TwitchChatDownloader.IntegrationTests
{
    [TestClass]
    public class ValidConfigTests
    {
        private readonly IConfiguration _configuration;

        public ValidConfigTests() =>
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("ValidSettings.json")
                .Build();

        [TestMethod]
        public void ShouldRetrieveTwitchConfig()
        {
            ITwitchConfig twitchConfig = _configuration.GetValidatableOrThrow<TwitchConfig>();
            Assert.AreEqual("TestClientId", twitchConfig.ClientId);
        }

        [TestMethod]
        public void ShouldRetrieveSrtFileConfig()
        {
            ISrtFileConfig srtFileConfig = _configuration.GetValidatableOrThrow<SrtFileConfig>();
            Assert.AreEqual(Path.Combine(Directory.GetCurrentDirectory(), "SrtFileFolder"), srtFileConfig.OutputDirectory.FullName);
        }

        [TestMethod]
        public void ShouldRetrieveSrtConfig()
        {
            ISrtConfig srtConfig = _configuration.GetValidatableOrThrow<SrtConfig>();
            Assert.AreEqual(1, srtConfig.MaxMessagesOnscreen);
            Assert.AreEqual(TimeSpan.FromSeconds(10), srtConfig.MaxTimeOnscreen);
            Assert.AreEqual(TimeSpan.FromMilliseconds(100), srtConfig.Delta);
        }
    }
}
