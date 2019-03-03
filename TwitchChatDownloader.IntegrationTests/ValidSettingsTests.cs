using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitchChatDownloader.Extensions;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.IntegrationTests
{
    [TestClass]
    public class ValidSettingsTests
    {
        private readonly IConfiguration _configuration;

        public ValidSettingsTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("ValidSettings.json")
                .Build();
        }

        [TestMethod]
        public void ShouldRetrieveTwitchSettings()
        {
            ITwitchSettings twitchSettings = _configuration.GetValidatableOrThrow<TwitchSettings>();
            Assert.AreEqual("TestClientId", twitchSettings.ClientId);
        }

        [TestMethod]
        public void ShouldRetrieveSrtFileSettings()
        {
            ISrtFileSettings srtFileSettings = _configuration.GetValidatableOrThrow<SrtFileSettings>();
            Assert.AreEqual(Path.Combine(Directory.GetCurrentDirectory(), "SrtFileFolder"), srtFileSettings.OutputDirectory.FullName);
        }

        [TestMethod]
        public void ShouldRetrieveSrtSettings()
        {
            ISrtSettings srtSettings = _configuration.GetValidatableOrThrow<SrtSettings>();
            Assert.AreEqual(1, srtSettings.MaxMessagesOnscreen);
            Assert.AreEqual(TimeSpan.FromSeconds(10), srtSettings.MaxTimeOnscreen);
            Assert.AreEqual(TimeSpan.FromMilliseconds(100), srtSettings.Delta);
        }
    }
}
