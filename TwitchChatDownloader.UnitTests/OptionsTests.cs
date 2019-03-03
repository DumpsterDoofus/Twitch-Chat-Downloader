using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TwitchChatDownloader.Models;
using TwitchLib.Api.Core.Enums;

namespace TwitchChatDownloader.UnitTests
{
    [TestClass]
    public class OptionsTests
    {
        private const string VideoTypeFlag = "--videotype";
        private const string VideoIdFlag = "--videoid";
        private const string UsernameFlag = "--username";
        
        [DataTestMethod]
        [DataRow("blastoise")]
        [DataRow("")]
        public void InvalidVideoTypeShouldNotParse(string videoType)
        {
            var args = new[] {VideoTypeFlag, videoType};
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Fail);
        }

        [TestMethod]
        public void NoArgumentsShouldParseSaneDefaults()
        {
            var args = new string[] {};
            const int expectedVideoId = 0;
            const VideoType expectedVideoType = VideoType.All;
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    Assert.AreEqual(expectedVideoId, options.VideoId);
                    Assert.AreEqual(expectedVideoType, options.VideoType);
                    Assert.IsNull(options.Username);
                })
                .WithNotParsed(Fail);
        }

        [DataTestMethod]
        [DataRow("All")]
        [DataRow("Upload")]
        [DataRow("Archive ")]
        [DataRow(" Highlight")]
        public void ValidVideoTypeShouldParse(string videoType)
        {
            var expected = Enum.Parse<VideoType>(videoType);
            var args = new[] { VideoTypeFlag, videoType };
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options => Assert.AreEqual(expected, options.VideoType))
                .WithNotParsed(Fail);
        }

        [DataTestMethod]
        [DataRow(13)]
        public void ValidVideoIdShouldParse(int videoId)
        {
            var args = new[] { VideoIdFlag, videoId.ToString() };
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options => Assert.AreEqual(videoId, options.VideoId))
                .WithNotParsed(Fail);
        }

        [DataTestMethod]
        [DataRow("zfg1")]
        public void ValidUsernameShouldParse(string username)
        {
            var args = new[] { UsernameFlag, username };
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options => Assert.AreEqual(username, options.Username))
                .WithNotParsed(Fail);
        }

        private static void Fail(IEnumerable<Error> errors) =>
            Assert.Fail($"Parsing failed, but was expected to succeed. \nErrors: {string.Join('\n', errors.Select(PrettyPrint))}");
        private static void Fail(Options options) =>
            Assert.Fail($"Parsing succeeded, but was expected to fail. \nOptions: {PrettyPrint(options)}");
        private static string PrettyPrint(object o) =>
            JsonConvert.SerializeObject(o, Formatting.Indented);
    }
}
