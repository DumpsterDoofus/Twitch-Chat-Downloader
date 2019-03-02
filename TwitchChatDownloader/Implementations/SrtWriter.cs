using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Implementations
{
    class SrtWriter : ISrtWriter
    {
        private readonly ISrtLineWriter _srtLineWriter;
        private readonly ISrtFileWriter _srtFileWriter;

        public SrtWriter(ISrtLineWriter srtLineWriter, ISrtFileWriter srtFileWriter)
        {
            _srtLineWriter = srtLineWriter ?? throw new ArgumentNullException(nameof(srtLineWriter));
            _srtFileWriter = srtFileWriter ?? throw new ArgumentNullException(nameof(srtFileWriter));
        }

        public async Task Write(InternalVideo video, IEnumerable<InternalComment> comments)
        {
            var srtLines = _srtLineWriter.Write(comments);
            var content = string.Join("",
                srtLines.Select((srtLine, index) =>
                    $"{index + 1}\n{srtLine.StartTime:hh\\:mm\\:ss\\,fff} --> {srtLine.EndTime:hh\\:mm\\:ss\\,fff}\n{srtLine.Message}\n"));
            await _srtFileWriter.Write(video, content);
        }
    }
}
