using System.IO;
using System.Threading.Tasks;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Models;

namespace TwitchChatDownloader.Implementations
{
    internal class SrtFileWriter : ISrtFileWriter
    {
        private readonly ISrtFileSettings _srtFileSettings;

        public SrtFileWriter(ISrtFileSettings srtFileSettings)
        {
            _srtFileSettings = srtFileSettings;
        }

        public async Task<FileInfo> Write(InternalVideo internalVideo, string content)
        {
            var safeName = string.Concat(internalVideo.Name.Split(Path.GetInvalidFileNameChars()));
            var path = Path.Combine(_srtFileSettings.OutputDirectory.FullName, $"{safeName}-v{internalVideo.Id}.srt");
            await File.WriteAllTextAsync(path, content).ConfigureAwait(false);
            return new FileInfo(path);
        }
    }
}
