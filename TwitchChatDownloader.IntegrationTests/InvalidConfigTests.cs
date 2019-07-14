using System;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitchChatDownloader.Config;
using TwitchChatDownloader.Extensions;

namespace TwitchChatDownloader.IntegrationTests
{
    [TestClass]
    public class InvalidConfigTests
    {
        private readonly IConfiguration _configuration;

        public InvalidConfigTests() =>
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("InvalidSettings.json")
                .Build();

        [TestMethod]
        public void InvalidConfigShouldThrow() =>
            Assert.ThrowsException<Exception>(() => _configuration.GetValidatableOrThrow<TwitchConfig>());

        [TestMethod]
        public void NonexistentConfigShouldThrow() =>
            Assert.ThrowsException<NullReferenceException>(() => _configuration.GetValidatableOrThrow<SrtFileConfig>());
    }
}
