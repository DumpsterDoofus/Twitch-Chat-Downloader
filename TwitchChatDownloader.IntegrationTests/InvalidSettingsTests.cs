using System;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitchChatDownloader.Extensions;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.IntegrationTests
{
    [TestClass]
    public class InvalidSettingsTests
    {
        private readonly IConfiguration _configuration;

        public InvalidSettingsTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("InvalidSettings.json")
                .Build();
        }

        [TestMethod]
        public void InvalidSettingsShouldThrow() =>
            Assert.ThrowsException<Exception>(() => _configuration.GetValidatableOrThrow<TwitchSettings>());

        [TestMethod]
        public void NonexistentSettingsShouldThrow() =>
            Assert.ThrowsException<NullReferenceException>(() => _configuration.GetValidatableOrThrow<SrtFileSettings>());
    }
}
