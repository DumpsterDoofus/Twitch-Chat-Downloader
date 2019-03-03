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

        public async Task Write(InternalVideo internalVideo, string content)
        {
            var safeName = string.Join("_", internalVideo.Name.Split(Path.GetInvalidFileNameChars()));
            await File.WriteAllTextAsync(Path.Combine(_srtFileSettings.OutputDirectory.FullName, $"{safeName}-v{internalVideo.Id}.srt"), content);
        }
    }
}
