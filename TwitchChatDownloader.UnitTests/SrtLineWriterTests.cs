using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitchChatDownloader.Implementations;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.UnitTests
{
    [TestClass]
    public class SrtLineWriterTests
    {
        [TestMethod]
        public void T()
        {
            var srtLineWriter = new SrtLineWriter(new TestSrtSettings
            {
                MaxMessagesOnscreen = 1,
                MaxTimeOnscreen = TimeSpan.FromSeconds(2.5),
                Delta = TimeSpan.Zero
            });
        }
    }
}
