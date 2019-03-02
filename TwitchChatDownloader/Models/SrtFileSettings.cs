using System.ComponentModel.DataAnnotations;
using System.IO;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Validation;

namespace TwitchChatDownloader.Models
{
    class SrtFileSettings : Validatable, ISrtFileSettings
    {
        [Required]
        [ValidDirectory]
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
