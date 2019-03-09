using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitchChatDownloader.Models;
using TwitchLib.Api.Core.Enums;

namespace TwitchChatDownloader.UnitTests
{
    [TestClass]
    public class UserOptionsTests
    {
        private const string VideoTypeFlag = "--videotype";
        private const string ValidUsername = "someusername";
        private const string UserVerb = "user";
        private const string UsernameFlag = "--username";
        private const string ValidVideoType = "archive";

        [TestMethod]
        public void NoArgumentsShouldNotParse()
        {
            var args = new[] { UserVerb };
            Parser.Default.ParseArguments<UserOptions>(args)
                .WithParsed(Fail);
        }

        [TestMethod]
        public void NoUsernameShouldNotParse()
        {
            var args = new[] { UserVerb, VideoTypeFlag, ValidVideoType };
            Parser.Default.ParseArguments<UserOptions>(args)
                .WithParsed(Fail);
        }

        [TestMethod]
        public void NoVideoTypeShouldUseDefault()
        {
            var args = new[] { UserVerb, UsernameFlag, ValidUsername };
            Parser.Default.ParseArguments<UserOptions>(args)
                .WithParsed(userOptions =>
                {
                    Assert.AreEqual(VideoType.All, userOptions.VideoType);
                    Assert.AreEqual(ValidUsername, userOptions.Username);
                })
                .WithNotParsed(Fail);
        }

        [DataTestMethod]
        [DataRow("blastoise")]
        [DataRow("")]
        public void InvalidVideoTypeShouldNotParse(string videoType)
        {
            var args = new[] { UserVerb, UsernameFlag, ValidUsername, VideoTypeFlag, videoType };
            Parser.Default.ParseArguments<UserOptions>(args)
                .WithParsed(Fail);
        }

        [DataTestMethod]
        [DataRow("All", VideoType.All)]
        [DataRow("Upload", VideoType.Upload)]
        [DataRow("Archive ", VideoType.Archive)]
        [DataRow(" Highlight", VideoType.Highlight)]
        public void ValidVideoTypeShouldParse(string videoTypeArg, VideoType videoType)
        {
            var args = new[] { UserVerb, UsernameFlag, ValidUsername, VideoTypeFlag, videoTypeArg };
            Parser.Default.ParseArguments<UserOptions>(args)
                .WithParsed(userOptions =>
                {
                    Assert.AreEqual(videoType, userOptions.VideoType);
                    Assert.AreEqual(ValidUsername, userOptions.Username);
                })
                .WithNotParsed(Fail);
        }

        private static void Fail(UserOptions options) =>
            Assert.Fail("Parsing succeeded, but was expected to fail.");

        private static void Fail(IEnumerable<Error> errors) =>
            Assert.Fail($"Parsing failed, but was expected to succeed. \nErrors: {string.Join('\n', errors.Select(error => error.ToString()))}");
    }
}
