using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.UnitTests
{
    [TestClass]
    public class VideoOptionsTests
    {

        private const string VideoIdFlag = "--videoid";
        private const string VideoVerb = "video";

        [TestMethod]
        public void NoVideoIdShouldFail()
        {
            var args = new[] { VideoVerb };
            Parser.Default.ParseArguments<VideoOptions>(args)
                .WithParsed(Fail);
        }

        [DataTestMethod]
        [DataRow("Blast")]
        public void InvalidVideoIdShouldFail(string videoId)
        {
            var args = new[] { VideoVerb, VideoIdFlag, videoId };
            Parser.Default.ParseArguments<VideoOptions>(args)
                .WithParsed(Fail);
        }

        [DataTestMethod]
        [DataRow(13)]
        public void ValidVideoIdShouldParse(int videoId)
        {
            var args = new[] { VideoVerb, VideoIdFlag, videoId.ToString() };
            Parser.Default.ParseArguments<VideoOptions>(args)
                .WithParsed(videoOptions => Assert.AreEqual(videoId, videoOptions.VideoId))
                .WithNotParsed(Fail);
        }

        private static void Fail(IEnumerable<Error> errors) =>
            Assert.Fail($"Parsing failed, but was expected to succeed. \nErrors: {string.Join('\n', errors.Select(error => error.ToString()))}");

        private static void Fail(VideoOptions videoOptions) =>
            Assert.Fail("Parsing succeeded, but was expected to fail.");
    }
}
