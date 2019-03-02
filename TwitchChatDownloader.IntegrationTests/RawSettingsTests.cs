//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using TwitchChatDownloader.Models;

//namespace TwitchChatDownloader.IntegrationTests
//{
//    [TestClass]
//    public class RawSettingsTests
//    {
//        private readonly RawSettings _rawSettings = ConfigurationAdapter.Bind<RawSettings>();

//        [TestMethod]
//        public void ShouldRetrieveTwitchSettings()
//        {
//            Assert.AreEqual("ClientId", _rawSettings.RawTwitchSettings.ClientId);
//        }

//        [TestMethod]
//        public void ShouldRetrieveFileSettings()
//        {
//            Assert.AreEqual("C:\\", _rawSettings.RawFileSettings.OutputDirectory);
//        }

//        [TestMethod]
//        public void ShouldRetrieveSrtSettings()
//        {
//            var srtSettings = _rawSettings.RawSrtSettings;
//            Assert.AreEqual(5, srtSettings.MaxMessagesOnscreen);
//            Assert.AreEqual(10, srtSettings.MaxSecondsOnscreen);
//            Assert.AreEqual(50, srtSettings.DeltaMilliseconds);
//        }
//    }
//}
