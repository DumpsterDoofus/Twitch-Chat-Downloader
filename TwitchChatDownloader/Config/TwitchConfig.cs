using System.ComponentModel.DataAnnotations;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Validation;

namespace TwitchChatDownloader.Config
{
    public class TwitchConfig : Validatable, ITwitchConfig
    {
        [Required]
        public string ClientId { get; set; }
    }
}
