using System.IO;
using TwitchChatDownloader.Validation;
using TwitchChatDownloader.Writers.SrtFile;

namespace TwitchChatDownloader.Config
{
    public class SrtFileConfig : Validatable, ISrtFileConfig
    {
        [ValidateDirectory]
        public string OutputDirectoryPath { get; set; }

        private DirectoryInfo _outputDirectory;
        public DirectoryInfo OutputDirectory
        {
            get
            {
                if (_outputDirectory == null && OutputDirectoryPath != null)
                {
                    _outputDirectory = new DirectoryInfo(OutputDirectoryPath);
                }
                return _outputDirectory;
            }
        }
    }
}
