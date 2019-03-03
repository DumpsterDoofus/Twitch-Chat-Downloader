# Twitch Chat Downloader

Have you ever downloaded a [Twitch](https://twitch.tv) video using [YouTube-DL](https://github.com/rg3/youtube-dl), watched it, and wished you could download the chat too? Now you can.

This console app downloads the chat on a Twitch video and converts it to an SRT file, so you can read chat as subtitles while watching the video. Here's it in action on VLC:

![](http://i.imgur.com/5thpdc8.jpg)

- Q: Why create this when [Chat Replay](https://help.twitch.tv/customer/portal/articles/2337148-chat-replay-faq) already exists?
 - A: Because Twitch Past Broadcasts are deleted after 30 days, and although you can download videos with YouTube-DL, at present you can't download the chat.

## How to use

1. Ensure the [.NET Core runtime](https://dotnet.microsoft.com/download) is on the path.
2. Download and unzip the [release](https://github.com/DumpsterDoofus/Twitch-Chat-Downloader/releases/download/1.0.0/TwitchChatDownloader.7z). 
3. In PowerShell (or similar terminal), run `dotnet TwitchChatDownloader.dll --help` to display the help.

You may need to replace the `appsettings.json`'s `TwitchClientId` in case the one in source control ever becomes invalid (but it should work for now). You can get these by [registering an application at Twitch](https://www.twitch.tv/kraken/oauth2/clients/new). 

> Security note: [The Twitch Client ID is not a secret](https://dev.twitch.tv/docs/authentication/), which is why I leave it in source control. Plz no abuserino!

### Examples

#### Downloading for a single video (https://www.twitch.tv/videos/69027652)
```
dotnet TwitchChatDownloader.dll --videoid 69027652
```

#### Downloading all highlights from a user's channel (https://www.twitch.tv/zfg1)

```
dotnet TwitchChatDownloader.dll --username zfg1 --videotype highlight
```
