using System;
using System.IO;
using System.Threading.Tasks;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Writers.SrtFile
{
    internal class SrtFileWriter : ISrtFileWriter
    {
        private readonly ISrtFileConfig _srtFileConfig;

        public SrtFileWriter(ISrtFileConfig srtFileConfig) =>
            _srtFileConfig = srtFileConfig ?? throw new ArgumentNullException(nameof(srtFileConfig));

        public async Task<FileInfo> Write(InternalVideo internalVideo, string content)
        {
            var safeName = string.Concat(internalVideo.Name.Split(Path.GetInvalidFileNameChars()));
            var path = Path.Combine(_srtFileConfig.OutputDirectory.FullName, $"{safeName}-v{internalVideo.Id}.srt");
            await File.WriteAllTextAsync(path, content);
            return new FileInfo(path);
        }
    }
}
