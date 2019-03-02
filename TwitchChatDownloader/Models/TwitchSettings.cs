using System.ComponentModel.DataAnnotations;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Validation;

namespace TwitchChatDownloader.Models
{
    class TwitchSettings : Validatable, ITwitchSettings
    {
        [Required]
        public string ClientId { get; set; }
    }
}
