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
    public class OptionsTests2
    {
        private void NotParsedFail(IEnumerable<Error> errors) => Assert.Fail($"Parsing failed, but was not expected to. \nErrors: {string.Join('\n', errors.Select(PrettyPrint))}");
        private void ParsedFail(Options options) => Assert.Fail($"Parsing succeeded, but was not expected to. \nOptions: {PrettyPrint(options)}");
        private void ParsedSucceed(Options options) {}
        private void NotParsedSucceed(IEnumerable<Error> errors) { }
        private const string VideoTypeFlag = "--videotype";
        private const string VideoIdFlag = "--videoid";
        private const string UsernameFlag = "--username";
        private const string OutputTypeFlag = "--outputtype";
        private static string PrettyPrint(object o)
        {
            return $"Type:\n{o}\nValue:\n{JsonConvert.SerializeObject(o, Formatting.Indented)}";
        }

        //TODO: Add validation around values. Might need infrastructure around detecting equality of expected vs actual results.

        [DataTestMethod]
        [DataRow("blastoise")]
        [DataRow("")]
        public void InvalidVideoTypeShouldNotParse(string videoType)
        {
            var args = new[] {VideoTypeFlag, videoType};
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(ParsedFail)
                .WithNotParsed(NotParsedSucceed);
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
                .WithNotParsed(NotParsedFail);
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
                .WithNotParsed(NotParsedFail);
        }

        [DataTestMethod]
        [DataRow(13)]
        public void ValidVideoIdShouldParse(int videoId)
        {
            var args = new[] { VideoIdFlag, videoId.ToString() };
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options => Assert.AreEqual(videoId, options.VideoId))
                .WithNotParsed(NotParsedFail);
        }

        [DataTestMethod]
        [DataRow("zfg1")]
        public void ValidUsernameShouldParse(string username)
        {
            var args = new[] { UsernameFlag, username };
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options => Assert.AreEqual(username, options.Username))
                .WithNotParsed(NotParsedFail);
        }
    }
}
